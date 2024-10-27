using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteers.ValueObjects;

public record PetColor : NotEmptyStringValueObject
{
    private PetColor(string value)
        : base(value)
    {
    }
    
    public static Result<PetColor, Error> Create(string value) 
        => Create(v => new PetColor(v), value, Constants.Pet.MaxColorLength);
    
    public static UnitResult<Error> Validate(string value) 
        => Validate(value, Constants.Pet.MaxColorLength);
}