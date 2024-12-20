using Microsoft.AspNetCore.Mvc;
using PetFinder.API.Controllers.Species.Requests;
using PetFinder.API.Extensions;
using PetFinder.Application.Features.Specles.Create;
using PetFinder.Application.Features.Specles.CreateBreed;

namespace PetFinder.API.Controllers.Species;

[ApiController]
[Route("[controller]")]
public class SpeciesController()
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateSpeciesRequest request,
        [FromServices] CreateSpeciesHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await handler.Handle(command, cancellationToken);
        
        return result.IsFailure 
            ? result.Error.ToResponse() 
            : Ok();
    }

    [HttpPost("{id:guid}/breeds")]
    public async Task<IActionResult> AddBreed(
        Guid id,
        [FromBody] CreateBreedRequest request,
        [FromServices] CreateBreedHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);

        var result = await handler.Handle(command, cancellationToken);
        
        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok();
    }
}