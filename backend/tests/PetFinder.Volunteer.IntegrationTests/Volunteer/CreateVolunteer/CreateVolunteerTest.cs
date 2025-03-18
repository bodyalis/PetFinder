using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFinder.Application.Features;
using PetFinder.Application.Features.Shared.Interfaces;

namespace PetFinder.Volunteer.IntegrationTests;

public class CreateVolunteerTest : BaseVolunteerTest
{
    private readonly ICommandHandlerWithResponse<CreateVolunteerCommand, Guid> _sut;

    public CreateVolunteerTest(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = ServiceScope.ServiceProvider
            .GetRequiredService<ICommandHandlerWithResponse<CreateVolunteerCommand, Guid>>();
    }

    [Fact]
    public async Task Create_volunteer()
    {
        // Arrange
        var command = Fixture.BuildCreateVolunteerCommand();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);
        var volunteer = result.IsSuccess
            ? await ReadDbContext.Volunteers.FirstOrDefaultAsync(x => x.Id == result.Value, CancellationToken.None)
            : null;

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        volunteer.Should().NotBeNull();
    }
}