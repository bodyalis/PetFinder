using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteers.ValueObjects;

public record PetName : NotEmptyStringValueObject
{
    private PetName(string value)
        : base(value)
    {
    }
    
    public static Result<PetName, Error> Create(string value) =>
        Create(v => new PetName(v), value, Constants.Pet.MaxNameLength);

    public static UnitResult<Error> Validate(string value) 
        => Validate(value, Constants.Pet.MaxNameLength);
}