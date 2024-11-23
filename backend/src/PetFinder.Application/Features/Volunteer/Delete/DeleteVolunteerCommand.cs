using PetFinder.Application.Features.Shared.Interfaces;

namespace PetFinder.Application.Features.Delete;

public record DeleteVolunteerCommand(Guid Id) : ICommand;