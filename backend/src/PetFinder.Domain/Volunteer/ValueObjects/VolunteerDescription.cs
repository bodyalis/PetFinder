using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteer.ValueObjects;

public record VolunteerDescription : NotEmptyStringValueObject
{
    private VolunteerDescription(string value)
        : base(value)
    {
    }

    public static Result<VolunteerDescription, Error> Create(string value)
    {
        return Create<VolunteerDescription>(
            v => new VolunteerDescription(v),
            value,
            Constants.Volunteer.MaxDescriptionLength);
    }

    public static UnitResult<Error> Validate(string value)
    {
        return Validate(value, Constants.Volunteer.MaxDescriptionLength);
    }
}