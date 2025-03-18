using Microsoft.Extensions.DependencyInjection;
using PetFinder.Application.Dto;
using PetFinder.Application.Features.GetWithPagination;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Application.Models;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.Volunteers.ValueObjects;

namespace PetFinder.Volunteer.IntegrationTests.GetVolunteersWithPagination;

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
    public Task Get_volunteers_with_pagination()
    {
        
    }
    
    private async Task<Guid> SeedVolunteer()
    {
        var volunteer = Domain.Volunteers.Models.Volunteer.Create
        (
            id: VolunteerId.New(),
            personName: PersonName.Create(firstName: "test", middleName: "test", lastName: "test").Value,
            phoneNumber: PhoneNumber.Create("+79999999999").Value,
            email: Email.Create("test@test.com").Value,
            experienceYears: 10,
            description: VolunteerDescription.Create("test").Value,
            socialNetworks:
            new ValueObjectList<SocialNetwork>([SocialNetwork.Create("title", "https://url.url").Value]),
            assistanceDetails: new ValueObjectList<AssistanceDetails>([
                AssistanceDetails.Create("title", "description").Value
            ])
        ).Value;

        await WriteDbContext.Volunteers.AddAsync(volunteer);
        await WriteDbContext.SaveChangesAsync();

        return volunteer.Id.Value;
    }
}