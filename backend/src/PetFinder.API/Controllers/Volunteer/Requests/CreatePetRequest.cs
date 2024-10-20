using PetFinder.Application.Features.CreatePet;

namespace PetFinder.API.Controllers.Volunteer.Requests;

public record CreatePetRequest(
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
    DateTime BirthDate,
    bool IsCastrated,
    bool IsVaccinated,
    string HelpStatus)
{
    public CreatePetCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, SpeciesId, BreedId, Address, Name, AnimalType, GeneralDescription, Color, HealthInformation,
            OwnerPhoneNumber, Weight, Height, DateOnly.FromDateTime(BirthDate), IsCastrated, IsVaccinated, HelpStatus);
}