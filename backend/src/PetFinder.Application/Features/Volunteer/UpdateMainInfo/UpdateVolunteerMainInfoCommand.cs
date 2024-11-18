using PetFinder.Application.Dto;

namespace PetFinder.Application.Features.UpdateMainInfo;

public record UpdateVolunteerMainInfoCommand(Guid Id, UpdateVolunteerMainInfoDto Dto);