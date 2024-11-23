using PetFinder.Application.Dto;
using PetFinder.Application.Features.Shared.Interfaces;

namespace PetFinder.Application.Features.UpdateMainInfo;

public record UpdateVolunteerMainInfoCommand(Guid Id, UpdateVolunteerMainInfoDto Dto) : ICommand;