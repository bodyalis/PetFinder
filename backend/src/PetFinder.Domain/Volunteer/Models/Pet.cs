using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.Enums;
using PetFinder.Domain.Shared.Exceptions;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.Shared.Interfaces;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteer.Enums;
using PetFinder.Domain.Volunteer.ValueObjects;

namespace PetFinder.Domain.Volunteer.Models;

public class Pet : 
    SharedKernel.Entity<PetId>,
    ISoftDeletable
{
    private readonly List<PetPhoto> _photos = [];

    private Pet(PetId id) : base(id)
    {
    }

    private Pet(
        PetId id,
        SpeciesBreedObject speciesBreedObject,
        PetName name,
        AnimalType animalType,
        PetGeneralDescription generalDescription,
        PetColor color,
        PetHealthInformation healthInformation,
        Address address,
        double weight,
        double height,
        PhoneNumber ownerPhoneNumber,
        DateOnly birthDate,
        bool isCastrated,
        bool isVaccinated,
        HelpStatusPet helpStatusPet) : base(id)
    {
        SpeciesBreedObject = speciesBreedObject;
        Name = name;
        AnimalType = animalType;
        GeneralDescription = generalDescription;
        Color = color;
        HealthInformation = healthInformation;
        Address = address;
        Weight = weight;
        Height = height;
        OwnerPhoneNumber = ownerPhoneNumber;
        BirthDate = birthDate;
        IsCastrated = isCastrated;
        IsVaccinated = isVaccinated;
        HelpStatus = helpStatusPet;
        DeletedAt = null;
        IsDeleted = false;  
    }

    public SpeciesBreedObject SpeciesBreedObject { get; private set; } = default!;
    public PetName Name { get; private set; } = default!;
    public AnimalType AnimalType { get; private set; } = default!;
    public PetGeneralDescription GeneralDescription { get; private set; } = default!;
    public PetColor Color { get; private set; } = default!;
    public PetHealthInformation HealthInformation { get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public double Weight { get; private set; }
    public double Height { get; private set; }
    public PhoneNumber OwnerPhoneNumber { get; private set; } = default!;
    public DateOnly BirthDate { get; private set; }
    public bool IsCastrated { get; private set; }
    public bool IsVaccinated { get; private set; }
    public HelpStatusPet HelpStatus { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyList<PetPhoto> Photos => _photos;
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public static Result<Pet, Error> Create(
        PetId id,
        SpeciesBreedObject speciesBreedObject,
        PetName name,
        AnimalType animalType,
        PetGeneralDescription generalDescription,
        PetColor color,
        PetHealthInformation healthInformation,
        Address address,
        double weight,
        double height,
        PhoneNumber ownerPhoneNumber,
        DateOnly birthDate,
        bool isCastrated,
        bool isVaccinated,
        HelpStatusPet helpStatusPet)
    {
        var validationResult = ValidateWeight(weight: weight);
        if (validationResult.IsFailure)
            return validationResult.Error;

        validationResult = ValidateHeight(height);
        if (validationResult.IsFailure)
            return validationResult.Error;
        
        validationResult = ValidateBirthDate(birthDate);
        if (validationResult.IsFailure)
            return validationResult.Error;

        return new Pet(
            id: id,
            speciesBreedObject: speciesBreedObject,
            name: name, 
            animalType: animalType,
            generalDescription: generalDescription,
            color: color,
            healthInformation: healthInformation,
            address: address,
            weight: weight,
            height: height,
            ownerPhoneNumber: ownerPhoneNumber,
            birthDate: birthDate,
            isCastrated: isCastrated,
            isVaccinated: isVaccinated,
            helpStatusPet: helpStatusPet);
    }

    public static UnitResult<Error> ValidateWeight(
        double weight)
    {
        if (weight < Constants.Pet.MinWeightValue)
            return Errors.General.ValueIsInvalid(
                nameof(Weight),
                StringHelper.GetValueLessThanNeedString(Constants.Pet.MinWeightValue));
        
        return UnitResult.Success<Error>();
    }

    public static UnitResult<Error> ValidateBirthDate(DateOnly birthDate)
    {
        if (birthDate > DateOnly.FromDateTime(DateTime.Now))
            return Errors.General.ValueIsInvalid(
                nameof(BirthDate),
                StringHelper.GetValueMoreThanNeedString("now"));
        
        return UnitResult.Success<Error>();
    }

    public static UnitResult<Error> ValidateHeight(double height)
    {
        if (height < Constants.Pet.MinHeightValue)
            return Errors.General.ValueIsInvalid(
                nameof(Height),
                StringHelper.GetValueMoreThanNeedString(Constants.Pet.MinHeightValue));
        
        return UnitResult.Success<Error>();
    }
    

    public void Activate()
    {
        EntityAlreadyActivatedException.ThrowIfActivated(!IsDeleted);

        IsDeleted = false;
        DeletedAt = null;
        
        _photos.ForEach(p => p.Activate());
    }

    public void Deactivate(DateTime deletedAt)
    {
        EntityAlreadyDeletedException.ThrowIfDeleted(IsDeleted);

        IsDeleted = true;
        DeletedAt = deletedAt;
        
        _photos.ForEach(p => p.Deactivate(deletedAt));
    }

    public void AddPhoto(PetPhoto petPhoto) => _photos.Add(petPhoto);
}