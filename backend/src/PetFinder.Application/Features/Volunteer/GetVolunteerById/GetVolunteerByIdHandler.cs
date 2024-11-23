using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFinder.Application.DataLayer;
using PetFinder.Application.Dto;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Domain.Shared;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteers.Models;

namespace PetFinder.Application.Features.GetVolunteerById;

public class GetVolunteerByIdHandler(
    ILogger<GetVolunteerByIdHandler> logger,
    IReadDbContext readDbContext) : IQueryHandler<GetVolunteerByIdQuery, VolunteerDto>
{
    public async Task<Result<VolunteerDto, ErrorList>> Handle(
        GetVolunteerByIdQuery query,
        CancellationToken cancellationToken)
    {
        var volunteer = await readDbContext.Volunteers.FindAsync([query.Id], cancellationToken);

        return volunteer is null
            ? Errors.General.RecordNotFound(nameof(Volunteer), query.Id).ToErrorList()
            : volunteer;
    }
}