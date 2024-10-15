using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteer.ValueObjects;

namespace PetFinder.Domain.Species.ValueObjects;

public record SpeciesTitle : NotEmptyStringValueObject
{
    private SpeciesTitle(string value)
        : base(value)
    {
    }

    public static Result<SpeciesTitle, Error> Create(string value)
    {
        return Create<SpeciesTitle>(
            v => new SpeciesTitle(v),
            value,
            Constants.Species.MaxTitleLength);
    }

    public static UnitResult<Error> Validate(string value)
    {
        return Validate(value, Constants.Species.MaxTitleLength);
    }
}