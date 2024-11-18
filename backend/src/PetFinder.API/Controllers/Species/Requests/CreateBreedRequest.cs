using PetFinder.Application.Features.Specles.CreateBreed;

namespace PetFinder.API.Controllers.Species.Requests;

public record CreateBreedRequest(string Title, string Description)
{
    public CreateBreedCommand ToCommand(Guid speciesId) 
        => new(speciesId, Title, Description);
}