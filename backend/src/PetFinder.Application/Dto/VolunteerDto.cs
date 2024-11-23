namespace PetFinder.Application.Dto;

public class VolunteerDto
{
    public Guid Id { get; set; }
    public PersonNameDto PersonName { get; } = default!;
    public string PhoneNumber { get;  } = default!;
    public string Email { get; } = default!;
    public int ExperienceYears { get; }
    public string Description { get; } = default!;
    public List<SocialNetworkDto> SocialNetworks { get; } = [];
    public List<AssistanceDetailsDto> AssistanceDetailsDtos { get; } = [];
    public List<PetDto> Pets { get; } = [];
    public bool IsDeleted { get;  }
    public DateTime? DeletedAt { get; }
}