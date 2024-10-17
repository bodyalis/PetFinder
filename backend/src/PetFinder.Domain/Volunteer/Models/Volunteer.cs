using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.Exceptions;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.Shared.Interfaces;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteer.Enums;
using PetFinder.Domain.Volunteer.ValueObjects;

namespace PetFinder.Domain.Volunteer.Models;

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

    public void AddPet(Pet pet)
    {
        ArgumentNullException.ThrowIfNull(pet);
        
        _pets.Add(pet);
    }
}