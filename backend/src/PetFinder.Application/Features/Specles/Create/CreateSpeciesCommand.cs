using PetFinder.Application.Features.Shared.Interfaces;

namespace PetFinder.Application.Features.Specles.Create;

public record CreateSpeciesCommand(string Title) : ICommand;