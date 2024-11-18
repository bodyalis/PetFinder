namespace PetFinder.Application.Dto;

public class VolunteerDto
{
    public Guid Id { get; set; }
    public PersonNameDto PersonName { get; } = default!;
    public string PhoneNumber { get;  } = default!;
    public string Email { get; } = default!;
    public int ExperienceYears { get; }
    public string Description { get; } = default!;
    public List<SocialNetworkDto> SocialNetworks { get; } = new List<SocialNetworkDto>();
    public List<AssistanceDetailsDto> AssistanceDetailsDtos { get; } = new List<AssistanceDetailsDto>();
    public List<PetDto> Pets { get; } = [];
    public bool IsDeleted { get;  }
    public DateTime? DeletedAt { get; }
}