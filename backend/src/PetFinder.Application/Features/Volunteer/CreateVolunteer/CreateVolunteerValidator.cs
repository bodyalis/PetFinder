using FluentValidation;
using PetFinder.Application.Extensions;
using PetFinder.Domain.Volunteers.Models;
using PetFinder.Domain.Volunteers.ValueObjects;

namespace PetFinder.Application.Features;

public class CreateVolunteerValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerValidator()
    {
        RuleFor(request => request.PersonNameDto)
            .MustBeValueObject(dto => PersonName.Validate(
                dto.FirstName,
                dto.MiddleName,
                dto.LastName)
            );

        RuleFor(request => request.Email).MustBeValueObject(Email.Validate);

        RuleFor(request => request.PhoneNumber).MustBeValueObject(PhoneNumber.Validate);

        RuleFor(request => request.Description).MustBeValueObject(VolunteerDescription.Validate);

        RuleForEach(request => request.SocialNetworkDtos)
            .MustBeValueObject(dto => SocialNetwork.Validate(
                dto.Title,
                dto.Url)
            );

        RuleForEach(request => request.AssistanceDetailsDtos)
            .MustBeValueObject(dto => AssistanceDetails.Validate(
                dto.Title,
                dto.Description)
            );

        RuleFor(request => request.ExperienceYears)
            .MustBeValueObject(Volunteer.ValidateExperienceYears);
    }
}