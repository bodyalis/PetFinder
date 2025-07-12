using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFinder.Application.Dto;
using PetFinder.Application.Features.GetVolunteerById;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Volunteer.IntegrationTests.Tests.GetVolunteerById;

public class GetVolunteerByIdTest : BaseVolunteerTest
{
    private readonly IQueryHandler<GetVolunteerByIdQuery,VolunteerDto> _sut;

    public GetVolunteerByIdTest(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = ServiceScope.ServiceProvider.GetRequiredService<IQueryHandler<GetVolunteerByIdQuery, VolunteerDto>>();
    }

    [Fact]
    public async Task Get_volunteer_by_id()
    {
        //Arrange
        var volunteerId = (await SeedManager.SeedVolunteers(1)).First().Id.Value;
        var command = new GetVolunteerByIdQuery(volunteerId);
        
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(volunteerId);
    }

    [Fact]
    public async Task Get_volunteer_by_empty_id()
    {
        //Arrange
        var command = new GetVolunteerByIdQuery(Guid.Empty);
        
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(Errors.General.RecordNotFound(nameof(Volunteer), command.Id).ToErrorList());
    }
    
}