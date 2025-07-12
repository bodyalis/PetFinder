using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFinder.Application.Dto;
using PetFinder.Application.Features.CreatePet;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Species.Models;
using PetFinder.Domain.Species.ValueObjects;
using PetFinder.Domain.Volunteers.ValueObjects;

namespace PetFinder.Volunteer.IntegrationTests.CreatePet;

public class CreatePetTest : BaseVolunteerTest
{
    private readonly ICommandHandlerWithResponse<CreatePetCommand, Guid> _sut;

    public CreatePetTest(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = ServiceScope.ServiceProvider.GetRequiredService<ICommandHandlerWithResponse<CreatePetCommand, Guid>>();
    }

    [Fact]
    public async Task Create_pet()
    {
        // Arrange
        var volunteerId = (await SeedManager.SeedVolunteers(1)).First().Id;
        var speciesId = await SeedSpecies();
        var breedId = await SeedBreed(speciesId);
        
        // Act
        var petCommand = Fixture.BuildCreatePetCommand(volunteerId, speciesId, breedId);

        var result = await _sut.Handle(petCommand, CancellationToken.None);
        var pet = result.IsSuccess 
            ? await ReadDbContext.Pets.FirstOrDefaultAsync(p => p.Id == result.Value)
            : null;
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        pet.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_pet_when_volunteer_not_exists()
    {
        // Arrange
        var volunteerId = Guid.Empty;
        var speciesId = await SeedSpecies();
        var breedId = await SeedBreed(speciesId);

        // Act
        var petCommand = Fixture.BuildCreatePetCommand(volunteerId, speciesId, breedId);
        var result = await _sut.Handle(petCommand, CancellationToken.None);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should()
            .BeEquivalentTo(Errors.General.RecordNotFound(nameof(Volunteer), nameof(VolunteerId)).ToErrorList());
    }

    private async Task<Guid> SeedSpecies()
    {
        var species = Species.Create(
            id: SpeciesId.New(),
            title: SpeciesTitle.Create("test").Value
        ).Value;

        await WriteDbContext.Species.AddAsync(species);
        await WriteDbContext.SaveChangesAsync();

        return species.Id.Value;
    }

    private async Task<Guid> SeedBreed(Guid speciesId)
    {
        var breed = Breed.Create(
            id: BreedId.New(),
            title: BreedTitle.Create("title").Value,
            description: BreedDescription.Create("description").Value,
            speciesId: SpeciesId.Create(speciesId)).Value;
        
        await WriteDbContext.Breeds.AddAsync(breed);
        await WriteDbContext.SaveChangesAsync();

        return breed.Id.Value;
    }
}