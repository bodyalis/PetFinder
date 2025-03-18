using PetFinder.Application.Dto;
using PetFinder.Application.Features.Shared.Interfaces;

namespace PetFinder.Application.Features;

public record CreateVolunteerCommand(
    PersonNameDto PersonNameDto,
    IEnumerable<SocialNetworkDto> SocialNetworkDtos,
    IEnumerable<AssistanceDetailsDto> AssistanceDetailsDtos,
    string PhoneNumber,
    int ExperienceYears,
    string Description,
    string Email) : ICommand;