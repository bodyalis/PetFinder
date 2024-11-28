using PetFinder.Domain.Volunteers.Enums;

namespace PetFinder.Application.Dto;

public class PetDto
{
    public Guid Id { get; set; }
    public Guid SpeciesId { get; set; }
    public Guid BreedId { get; set; }
    public string Name { get; set; } = default!;
    public string GeneralDescription { get; set; } = default!;
    public string Color { get; set; } = default!;
    public string HealthInformation { get; set; } = default!;
    public AddressDto Address { get; set; } = default!;
    public double Weight { get; set; } 
    public double Height { get; set; }
    public string OwnerPhoneNumber { get; set; } = default!;
    public DateOnly BirthDate { get; set; }
    public bool IsCastrated { get; set; }
    public bool IsVaccinated { get; set; }
    public int OrderNumber { get; set; }
    public HelpStatusPet HelpStatus { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; } 

}