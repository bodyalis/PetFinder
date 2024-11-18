using PetFinder.Application.Features.Specles.Create;

namespace PetFinder.API.Controllers.Species.Requests;

public record CreateSpeciesRequest(string Title)
{
    public CreateSpeciesCommand ToCommand() => new CreateSpeciesCommand(Title);
}