using PetFinder.Domain.Volunteers.Enums;

namespace PetFinder.Application.Dto;

public class PetDto
{
    public Guid Id { get; init; }
    public Guid SpeciesId { get; init; }
    public Guid BreedId { get; init; }
    public string Name { get; init; } = default!;
    public string GeneralDescription { get; init; } = default!;
    public string Color { get; init; } = default!;
    public string HealthInformation { get; init; } = default!;
    public AddressDto Address { get; init; } = default!;
    public double Weight { get; init; }
    public double Height { get; init; }
    public string OwnerPhoneNumber { get; init; } = default!;
    public DateOnly BirthDate { get; init; }
    public bool IsCastrated { get; init; }
    public bool IsVaccinated { get; init; }
    public int OrderNumber { get; init; }
    public HelpStatusPet HelpStatus { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
}