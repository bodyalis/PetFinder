namespace PetFinder.Application.Dto;

public class VolunteerDto
{
    public Guid Id { get; init; }
    public PersonNameDto PersonName { get; init; } = default!;
    public string PhoneNumber { get; init; } = default!;
    public string Email { get; init; } = default!;
    public int ExperienceYears { get; init; }
    public string Description { get; init; } = default!;
    public List<SocialNetworkDto> SocialNetworks { get; init; } = [];
    public List<AssistanceDetailsDto> AssistanceDetailsDtos { get; init; } = [];
    public List<PetDto> Pets { get; init; } = [];
    public bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
}