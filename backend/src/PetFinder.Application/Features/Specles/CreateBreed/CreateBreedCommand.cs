namespace PetFinder.Application.Features.Specles.CreateBreed;

public record CreateBreedCommand(Guid SpeciesId, string Title, string Description);