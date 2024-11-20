using Microsoft.AspNetCore.Mvc;
using PetFinder.API.Controllers.Volunteer.Requests;
using PetFinder.API.Extensions;
using PetFinder.API.Processors;
using PetFinder.Application.Dto;
using PetFinder.Application.Features;
using PetFinder.Application.Features.AddPetPhotos;
using PetFinder.Application.Features.CreatePet;
using PetFinder.Application.Features.Delete;
using PetFinder.Application.Features.GetWithPagination;
using PetFinder.Application.Features.UpdateMainInfo;

namespace PetFinder.API.Controllers.Volunteer;

[ApiController]
[Route("[controller]")]
public class VolunteerController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] CreateVolunteerHandler handler,
        [FromBody] CreateVolunteerCommand createVolunteerCommand,
        [FromServices] ILogger<VolunteerController> logger,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(createVolunteerCommand, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPatch("{id:guid}/main-info")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerMainInfoDto dto,
        [FromServices] UpdateVolunteerMainInfoHandler handler,
        [FromServices] ILogger<VolunteerController> logger,
        CancellationToken cancellationToken = default)
    {
        var request = new UpdateVolunteerMainInfoCommand(id, dto);

        var result = await handler.Handle(request, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteVolunteerHandler handler,
        [FromServices] ILogger<VolunteerController> logger,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(id, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pets")]
    public async Task<IActionResult> CreatePet(
        [FromRoute] Guid volunteerId,
        [FromBody] CreatePetRequest request,
        [FromServices] CreatePetHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(volunteerId);
        var result = await handler.Handle(command, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok(result.Value);
    }

    [HttpPost("{volunteerId:guid}/pet/{petId:guid}/photos")]
    public async Task<IActionResult> AddPetPhotos(
        [FromRoute] Guid volunteerId,
        [FromRoute] Guid petId,
        [FromForm] IFormFileCollection fileCollection,
        [FromServices] AddPetPhotosHandler handler,
        CancellationToken cancellationToken)
    {
        using var fileProcessor = new FormFileProcessor(fileCollection.Count);

        var fileDtos = fileProcessor.Process(fileCollection);

        var command = new AddPetPhotosCommand(volunteerId, petId, fileDtos);

        var result = await handler.Handle(command, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] GetVolunteersWithPaginationRequest request,
        [FromServices] GetVolunteersWithPaginationHandler handler,
        CancellationToken cancellationToken
    )
    {
        var query = request.ToQuery();
        
        var result = await handler.Handle(query, cancellationToken);

        return result.IsFailure
            ? result.Error.ToResponse()
            : Ok();
    }
}