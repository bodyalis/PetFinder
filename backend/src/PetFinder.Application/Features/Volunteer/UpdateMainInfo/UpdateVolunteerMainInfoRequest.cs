using PetFinder.Application.Dto;

namespace PetFinder.Application.Features.UpdateMainInfo;

public record UpdateVolunteerMainInfoRequest(Guid Id, UpdateVolunteerMainInfoDto Dto);

public record UpdateVolunteerMainInfoDto(
    PersonNameDto PersonNameDto,
    string VolunteerDescription,
    string PhoneNumber,
    string Email,
    int ExperienceYears);