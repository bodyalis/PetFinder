using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFinder.Application.Dto;
using PetFinder.Application.Features.GetWithPagination;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Application.Models;
using PetFinder.Domain.Shared;

namespace PetFinder.Volunteer.IntegrationTests.Tests.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationTest : BaseVolunteerTest
{
    private readonly IQueryHandler<GetVolunteersWithPaginationQuery, PagedList<VolunteerDto>> _sut;

    public GetVolunteersWithPaginationTest(IntegrationTestsWebFactory factory)
        : base(factory)
    {
        _sut = ServiceScope.ServiceProvider.GetRequiredService<
            IQueryHandler<GetVolunteersWithPaginationQuery, PagedList<VolunteerDto>>>();
    }

    [Fact]
    public async Task Get_volunteers_with_pagination_()
    {
        const int pageSize10 = 10;
        const int totalcount = 20;

        // Arrange
        List<Domain.Volunteers.Models.Volunteer> volunteers = await SeedManager.SeedVolunteers(totalcount);
        GetVolunteersWithPaginationQuery query10 = new(1, 10);

        //Act
        Result<PagedList<VolunteerDto>, ErrorList> resultWith10Elements =
            await _sut.Handle(query10, CancellationToken.None);

        //Assert
        resultWith10Elements.Should().NotBeNull();
        resultWith10Elements.IsSuccess.Should().BeTrue();
        resultWith10Elements.Value.Items.Count.Should().Be(pageSize10);
        resultWith10Elements.Value.TotalCount.Should().Be(totalcount);

    }
}