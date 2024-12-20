using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.Exceptions;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.Shared.Interfaces;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteers.Enums;
using PetFinder.Domain.Volunteers.ValueObjects;

namespace PetFinder.Domain.Volunteers.Models;

public class Volunteer :
    SharedKernel.Entity<VolunteerId>,
    ISoftDeletable

{
    private readonly List<Pet> _pets = default!;

    private Volunteer(VolunteerId id)
        : base(id)
    {
    }

    private Volunteer(
        VolunteerId id,
        PersonName personName,
        PhoneNumber phoneNumber,
        Email email,
        int experienceYears,
        VolunteerDescription description,
        ValueObjectList<SocialNetwork> socialNetworks,
        ValueObjectList<AssistanceDetails> assistanceDetails) : base(id)
    {
        PersonName = personName;
        PhoneNumber = phoneNumber;
        ExperienceYears = experienceYears;
        Description = description;
        Email = email;
        SocialNetworks = socialNetworks;
        AssistanceDetails = assistanceDetails;
        _pets = [];
        DeletedAt = null;
        IsDeleted = false;
    }

    public PersonName PersonName { get; private set; } = default!;
    public PhoneNumber PhoneNumber { get; private set; } = default!;
    public int ExperienceYears { get; private set; }
    public VolunteerDescription Description { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public ValueObjectList<SocialNetwork> SocialNetworks { get; private set; } = default!;
    public ValueObjectList<AssistanceDetails> AssistanceDetails { get; private set; } = default!;
    public IReadOnlyList<Pet> Pets => _pets;

    public int PetsFoundHomeCount => Pets.Count(p => p.HelpStatus == HelpStatusPet.FoundHome);
    public int PetsLookingForHomeCount => Pets.Count(p => p.HelpStatus == HelpStatusPet.LookingForHome);
    public int PetsOnTreatmentCount => Pets.Count(p => p.HelpStatus == HelpStatusPet.OnTreatment);

    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public void Activate()
    {
        EntityAlreadyActivatedException.ThrowIfActivated(!IsDeleted);

        IsDeleted = false;
        DeletedAt = null;
    }

    public void Deactivate(DateTime deletedAt)
    {
        EntityAlreadyDeletedException.ThrowIfDeleted(IsDeleted);

        IsDeleted = true;
        DeletedAt = deletedAt;

        _pets.ForEach(p => p.Deactivate(deletedAt));
    }


    public void UpdateMainInfo(
        PersonName personName,
        PhoneNumber phoneNumber,
        Email email,
        VolunteerDescription description,
        int experienceYears)
    {
        PersonName = personName;
        PhoneNumber = phoneNumber;
        Email = email;
        Description = description;
        ExperienceYears = experienceYears;
    }


    public static Result<Volunteer, Error> Create(
        VolunteerId id,
        PersonName personName,
        PhoneNumber phoneNumber,
        Email email,
        int experienceYears,
        VolunteerDescription description,
        ValueObjectList<SocialNetwork> socialNetworks,
        ValueObjectList<AssistanceDetails> assistanceDetails)
    {
        if (experienceYears < Constants.Volunteer.MinExperienceYears)
            return Errors.General.ValueIsInvalid(
                nameof(experienceYears),
                $"Must be more or equal to {Constants.Volunteer.MinExperienceYears}");

        return new Volunteer(
            id: id,
            personName: personName,
            phoneNumber: phoneNumber,
            email: email,
            experienceYears: experienceYears,
            description: description,
            socialNetworks: socialNetworks,
            assistanceDetails: assistanceDetails
        );
    }

    public static UnitResult<Error> ValidateExperienceYears(int experienceYears)
        => UnitResult.FailureIf(experienceYears < Constants.Volunteer.MinExperienceYears,
            Errors.General.ValueIsInvalid(
                nameof(ExperienceYears),
                $"Must be more or equal to {Constants.Volunteer.MinExperienceYears}"));

    public UnitResult<Error> AddPet(Pet pet)
    {
        ArgumentNullException.ThrowIfNull(pet);

        var nextByOrderNumber = _pets.Where(p => p.OrderNumber.Value >= pet.OrderNumber.Value).ToList();

        var validationResult = nextByOrderNumber.Select(p => PetOrderNumber.Validate(p.OrderNumber.Value + 1)).ToList();
        if (validationResult.Any(r => r.IsFailure))
            return validationResult.First(r => r.IsFailure).Error;

        nextByOrderNumber.ForEach(p => p.SetNewOrderNumber(
            PetOrderNumber.Create(p.OrderNumber.Value + 1).Value));

        _pets.Add(pet);

        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> MovePet(Pet pet, PetOrderNumber newOrderNumber)
    {
        if (newOrderNumber.Value > _pets.Max(p => p.OrderNumber.Value))
            return Errors.General.ValueIsInvalid(nameof(PetOrderNumber), "The value exceeds the allowed value");
        
        if (pet.OrderNumber == newOrderNumber)
            return UnitResult.Success<Error>();

        if (pet.OrderNumber.Value < newOrderNumber.Value)
        {
            var nextByOrderNumber = _pets.Where(p => p.OrderNumber.Value > pet.OrderNumber.Value
                                                     && p.OrderNumber.Value <= newOrderNumber.Value).ToList();
            
            var validationResult = nextByOrderNumber
                .Select(p => PetOrderNumber.Validate(p.OrderNumber.Value - 1)).ToList();
            if (validationResult.Any(r => r.IsFailure))
                return validationResult.First(r => r.IsFailure).Error;
            
            nextByOrderNumber.ForEach(p => p.SetNewOrderNumber(
                PetOrderNumber.Create(p.OrderNumber.Value - 1).Value));
            
            pet.SetNewOrderNumber(newOrderNumber);
        }
        else
        {
            var nextByOrderNumber = _pets.Where(p => p.OrderNumber.Value < pet.OrderNumber.Value).ToList();
            
            var validationResult = nextByOrderNumber
                .Select(p => PetOrderNumber.Validate(p.OrderNumber.Value + 1)).ToList();
            if (validationResult.Any(r => r.IsFailure))
                return validationResult.First(r => r.IsFailure).Error;
            
            nextByOrderNumber.ForEach(p => p.SetNewOrderNumber(
                PetOrderNumber.Create(p.OrderNumber.Value + 1).Value));
            
            pet.SetNewOrderNumber(newOrderNumber);
        }

        return UnitResult.Success<Error>();
    }

    public Result<Pet, Error> GetPetById(PetId petId)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == petId);
        return pet is null
            ? Errors.General.RecordNotFound(nameof(Pet), nameof(PetId))
            : pet;
    }
}