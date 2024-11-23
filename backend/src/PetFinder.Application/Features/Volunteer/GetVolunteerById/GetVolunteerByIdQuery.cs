using PetFinder.Application.Features.Shared.Interfaces;

namespace PetFinder.Application.Features.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid Id) : IQuery;