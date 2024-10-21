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
        new(
            VolunteerId: volunteerId,
            SpeciesId: SpeciesId,
            BreedId: BreedId,
            Address: Address,
            Name: Name,
            AnimalType: AnimalType,
            GeneralDescription: GeneralDescription,
            Color: Color,
            HealthInformation: HealthInformation,
            OwnerPhoneNumber: OwnerPhoneNumber,
            Weight: Weight,
            Height: Height,
            BirthDate: DateOnly.FromDateTime(BirthDate),
            IsCastrated: IsCastrated,
            IsVaccinated: IsVaccinated,
            HelpStatus: HelpStatus);
}