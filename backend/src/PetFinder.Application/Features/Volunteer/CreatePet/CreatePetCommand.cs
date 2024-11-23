using PetFinder.Application.Dto;
using PetFinder.Application.Features.Shared.Interfaces;

namespace PetFinder.Application.Features.CreatePet;

public record CreatePetCommand(
    Guid VolunteerId,
    Guid SpeciesId,
    Guid BreedId,
    AddressDto Address,
    string Name,
    string AnimalType,
    string GeneralDescription,
    string Color,
    string HealthInformation,
    string OwnerPhoneNumber,
    double Weight,
    double Height,
    DateOnly BirthDate,
    bool IsCastrated,
    bool IsVaccinated,
    string HelpStatus) : IHandler;