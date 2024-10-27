using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteers.ValueObjects;

public record VolunteerDescription : NotEmptyStringValueObject
{
    private VolunteerDescription(string value)
        : base(value)
    {
    }

    public static Result<VolunteerDescription, Error> Create(string value) =>
        Create<VolunteerDescription>(
            v => new VolunteerDescription(v),
            value,
            Constants.Volunteer.MaxDescriptionLength);

    public static UnitResult<Error> Validate(string value)
        => Validate(value, Constants.Volunteer.MaxDescriptionLength);
}