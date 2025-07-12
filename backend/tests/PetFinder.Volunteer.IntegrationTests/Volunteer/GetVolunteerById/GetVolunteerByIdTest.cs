using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFinder.Application.Dto;
using PetFinder.Application.Features.GetVolunteerById;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.Volunteers.ValueObjects;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Volunteer.IntegrationTests.GetVolunteerById;

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
        var volunteer = await SeedVolunteer();
        var command = new GetVolunteerByIdQuery(volunteer);
        
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Id.Should().Be(volunteer);
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