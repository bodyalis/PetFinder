using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteers.ValueObjects;

public record PetGeneralDescription : NotEmptyStringValueObject
{
    private PetGeneralDescription(string value)
        : base(value)
    {
    }
    
    public static Result<PetGeneralDescription, Error> Create(string value) 
        => Create(v => new PetGeneralDescription(v), value, Constants.Pet.MaxGeneralDescriptionLength);
    
    public static UnitResult<Error> Validate(string value) 
        => Validate(value, Constants.Pet.MaxGeneralDescriptionLength);
}