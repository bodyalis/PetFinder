using PetFinder.Application.Dto;

namespace PetFinder.Application.Features;

public record CreateVolunteerRequest(
    PersonNameDto PersonNameDto,
    IEnumerable<SocialNetworkDto> SocialNetworkDtos,
    IEnumerable<AssistanceDetailsDto> AssistanceDetailsDtos,
    string PhoneNumber,
    int ExperienceYears,
    string Description,
    string Email);