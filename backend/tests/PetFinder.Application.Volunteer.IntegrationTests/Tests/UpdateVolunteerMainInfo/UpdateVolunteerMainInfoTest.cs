using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFinder.Application.Dto;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Application.Features.UpdateMainInfo;
using PetFinder.Domain.Shared;

namespace PetFinder.Volunteer.IntegrationTests.Tests.UpdateVolunteerMainInfo;

public class UpdateVolunteerMainInfoTest : BaseVolunteerTest
{
    private readonly ICommandHandler<UpdateVolunteerMainInfoCommand> _sut;

    public UpdateVolunteerMainInfoTest(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = ServiceScope.ServiceProvider.GetRequiredService<ICommandHandler<UpdateVolunteerMainInfoCommand>>();
    }

    [Fact]
    public async Task Update_volunteer_main_info()
    {
        // Arrangв
        Domain.Volunteers.Models.Volunteer volunteer = (await SeedManager.SeedVolunteers(1)).First();

        UpdateVolunteerMainInfoDto dto = Fixture.BuildUpdateVolunteerMainInfoDto();
        UpdateVolunteerMainInfoCommand command = new UpdateVolunteerMainInfoCommand(volunteer.Id.Value, dto);

        // Act
        UnitResult<ErrorList> result = await _sut.Handle(command, CancellationToken.None);
        VolunteerDto volunteerDto = await ReadDbContext.Volunteers.FirstAsync(v => v.Id == volunteer.Id.Value);


        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteerDto.PersonName.Should().BeEquivalentTo(dto.PersonNameDto);
        volunteerDto.Description.Should().BeEquivalentTo(dto.VolunteerDescription);
        volunteerDto.Email.Should().BeEquivalentTo(dto.Email);
        volunteerDto.PhoneNumber.Should().BeEquivalentTo(dto.PhoneNumber);
        volunteerDto.ExperienceYears.Should().Be(dto.ExperienceYears);
    }
}