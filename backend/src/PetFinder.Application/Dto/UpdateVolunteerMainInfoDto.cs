namespace PetFinder.Application.Dto;

public record UpdateVolunteerMainInfoDto(
    PersonNameDto PersonNameDto,
    string VolunteerDescription,
    string PhoneNumber,
    string Email,
    int ExperienceYears);