using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteers.ValueObjects;

public record PetHealthInformation : NotEmptyStringValueObject
{
    private PetHealthInformation(string value)
        : base(value)
    {
    }
    
    public static Result<PetHealthInformation, Error> Create(string value) 
        => Create(v => new PetHealthInformation(v), value, Constants.Pet.MaxHealthInformationLength);
    
    public static UnitResult<Error> Validate(string value) 
        => Validate(value, Constants.Pet.MaxHealthInformationLength);
}