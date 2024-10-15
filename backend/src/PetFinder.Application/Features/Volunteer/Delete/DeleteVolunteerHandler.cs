using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFinder.Application.DataLayer;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Domain.Shared;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteer.Models;

namespace PetFinder.Application.Features.Delete;

public class DeleteVolunteerHandler(
    IVolunteerRepository volunteerRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteVolunteerHandler> logger) : IHandler
{
    public async Task<Result<Guid, ErrorList>> Handle(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.Create(id);
        
        var volunteerByIdResult = await volunteerRepository.GetById(volunteerId, cancellationToken);
        if (volunteerByIdResult.IsFailure)
            return Errors.General.RecordNotFound(nameof(Volunteer), id).ToErrorList();

        var volunteer = volunteerByIdResult.Value;
        
        volunteerRepository.Delete(volunteer);
        
        await unitOfWork.SaveChanges(cancellationToken);
        return volunteer.Id.Value;
    }
}