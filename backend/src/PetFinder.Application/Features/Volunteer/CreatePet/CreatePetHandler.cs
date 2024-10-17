using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFinder.Application.DataLayer;
using PetFinder.Application.Extensions;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Application.Features.Specles;
using PetFinder.Domain.Shared;
using PetFinder.Domain.Shared.Enums;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Species.Models;
using PetFinder.Domain.Volunteer.Enums;
using PetFinder.Domain.Volunteer.Models;
using PetFinder.Domain.Volunteer.ValueObjects;

namespace PetFinder.Application.Features.CreatePet;

public class CreatePetHandler(
    IValidator<CreatePetCommand> validator,
    IVolunteerRepository volunteerRepository,
    ISpeciesRepository speciesRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreatePetHandler> logger) : IHandler
{
    public async Task<Result<Guid, ErrorList>> Handle(
        CreatePetCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.Errors.ToErrorList();

        var volunteerResult = await volunteerRepository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var breedExists = await speciesRepository.CheckExistsBreedBySpeciesIdAndBreedId(
            SpeciesId.Create(command.SpeciesId),
            BreedId.Create(command.BreedId),
            cancellationToken);
        if (!breedExists)
            return Errors.General.RecordNotFound(nameof(Breed), nameof(BreedId)).ToErrorList();

        var id = PetId.New();
        var name = PetName.Create(command.Name).Value;
        var speciesBreedObject = SpeciesBreedObject.Create(SpeciesId.Create(command.SpeciesId),
            BreedId.Create(command.BreedId)).Value;
        var animalType = Enum.Parse<AnimalType>(command.AnimalType, true);
        var helpStatusPet = Enum.Parse<HelpStatusPet>(command.HelpStatus, true);
        var generalDescription = PetGeneralDescription.Create(command.GeneralDescription).Value;
        var color = PetColor.Create(command.Color).Value;
        var healthInformation = PetHealthInformation.Create(command.HealthInformation).Value;
        var address = command.Address.ToValueObject().Value;
        var ownerPhoneNumber = PhoneNumber.Create(command.OwnerPhoneNumber).Value;


        var pet = Pet.Create(
            id: id,
            speciesBreedObject: speciesBreedObject,
            name: name,
            animalType: animalType,
            generalDescription: generalDescription,
            color: color,
            healthInformation: healthInformation,
            address: address,
            weight: command.Weight,
            height: command.Height,
            ownerPhoneNumber: ownerPhoneNumber,
            birthDate: command.BirthDate,
            isCastrated: command.IsCastrated,
            isVaccinated: command.IsVaccinated,
            helpStatusPet: helpStatusPet
        ).Value;

        volunteerResult.Value.AddPet(pet);

        await unitOfWork.SaveChanges(cancellationToken);

        logger.LogInformation($"Pet with id {pet.Id.Value} was created successfully");
        
        return pet.Id.Value;
    }
}