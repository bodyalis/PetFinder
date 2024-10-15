using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Species.ValueObjects;

public record BreedDescription : NotEmptyStringValueObject
{
    private BreedDescription(string value)
        : base(value)
    {
    }

    public static Result<BreedDescription, Error> Create(string value) =>
        Create(
            v => new BreedDescription(v),
            value,
            Constants.Breed.MaxDescriptionLength);

    public static UnitResult<Error> Validate(string value) 
        => Validate(value, Constants.Breed.MaxDescriptionLength);
}