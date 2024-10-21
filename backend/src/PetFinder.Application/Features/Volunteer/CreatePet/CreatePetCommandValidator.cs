using FluentValidation;
using PetFinder.Application.Extensions;
using PetFinder.Domain.Volunteer.Enums;
using PetFinder.Domain.Volunteer.Models;
using PetFinder.Domain.Volunteer.ValueObjects;

namespace PetFinder.Application.Features.CreatePet;

public class CreatePetCommandValidator : AbstractValidator<CreatePetCommand>
{
    public CreatePetCommandValidator()
    {
        RuleFor(command => command.HealthInformation).MustBeValueObject(PetHealthInformation.Validate);
        RuleFor(command => command.Name).MustBeValueObject(PetName.Validate);
        RuleFor(command => command.GeneralDescription).MustBeValueObject(PetGeneralDescription.Validate);
        RuleFor(command => command.Color).MustBeValueObject(PetColor.Validate);
        RuleFor(command => command.OwnerPhoneNumber).MustBeValueObject(PhoneNumber.Validate);
        
        RuleFor(command => command.HelpStatus).MustBeEnum(typeof(HelpStatusPet));
        
        RuleFor(command => command.BirthDate).MustBeValueObject(Pet.ValidateBirthDate);
        RuleFor(command => command.Height).MustBeValueObject(Pet.ValidateHeight);
        RuleFor(command => command.Weight).MustBeValueObject(Pet.ValidateWeight);
        
        RuleFor(command => command.Address).MustBeValueObject(
            a => Address.Validate(a.Country, a.City, a.Street, a.House, a.Description));
    }
}