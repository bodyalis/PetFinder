using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFinder.Application.DataLayer;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Domain.Shared;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteers.Models;

namespace PetFinder.Application.Features.Delete;

public class DeleteVolunteerHandler(
    IVolunteerRepository volunteerRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteVolunteerHandler> logger) : ICommandHandler<DeleteVolunteerCommand>
{
    public async Task<UnitResult<ErrorList>> Handle(
        DeleteVolunteerCommand command,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(command.Id);

        var volunteerByIdResult = await volunteerRepository.GetById(volunteerId, cancellationToken);
        if (volunteerByIdResult.IsFailure)
            return Errors.General.RecordNotFound(nameof(Volunteer), command.Id).ToErrorList();

        var volunteer = volunteerByIdResult.Value;

        volunteerRepository.Delete(volunteer);

        await unitOfWork.SaveChanges(cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}