using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Species.ValueObjects;

public record BreedTitle : NotEmptyStringValueObject
{
    private BreedTitle(string value)
        : base(value)
    {
    }

    public static Result<BreedTitle, Error> Create(string value) =>
        Create(
            v => new BreedTitle(v),
            value,
            Constants.Breed.MaxTitleLength);

    public static UnitResult<Error> Validate(string value) 
        => Validate(value, Constants.Breed.MaxTitleLength);
}