using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFinder.Application.Features.Delete;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Volunteer.IntegrationTests.Tests.DeleteVolunteer;

public class DeleteVolunteerTest : BaseVolunteerTest
{
    private readonly ICommandHandler<DeleteVolunteerCommand> _sut;

    public DeleteVolunteerTest(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = ServiceScope.ServiceProvider.GetRequiredService<ICommandHandler<DeleteVolunteerCommand>>();
    }

    [Fact]
    public async Task Delete_volunteer()
    {
        //Arrange
        var volunteerId = (await SeedManager.SeedVolunteers(1)).First().Id.Value;
        var command = new DeleteVolunteerCommand(volunteerId);

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        var volunteer =
            await ReadDbContext.Volunteers.FirstOrDefaultAsync(x => x.Id == volunteerId, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Should().BeNull();
    }
    
    [Fact]
    public async Task Delete_volunteer_when_not_exists()
    {
        //Arrange
        var volunteerId = Guid.Empty;
        var command = new DeleteVolunteerCommand(volunteerId);

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        var volunteer =
            await ReadDbContext.Volunteers.FirstOrDefaultAsync(x => x.Id == volunteerId, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should()
            .BeEquivalentTo(Errors.General.RecordNotFound(nameof(Volunteer), nameof(VolunteerId)).ToErrorList());
    }
}