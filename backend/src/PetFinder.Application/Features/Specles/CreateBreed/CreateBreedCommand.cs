using PetFinder.Application.Features.Shared.Interfaces;

namespace PetFinder.Application.Features.Specles.CreateBreed;

public record CreateBreedCommand(Guid SpeciesId, string Title, string Description) : ICommand;