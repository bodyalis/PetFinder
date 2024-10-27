using CSharpFunctionalExtensions;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteers.ValueObjects;

public record PetOrderNumber
{
    private PetOrderNumber()
    {
    }

    private PetOrderNumber(int value)
    {
        Value = value;
    }
    
    public int Value { get; }

    public static Result<PetOrderNumber, Error> CreateFirst() 
        => Create(Constants.Pet.MinOrderNumber);
    
    public static Result<PetOrderNumber, Error> Create(int number)
    {
        var validationResult = Validate(number);
        if (validationResult.IsFailure)
            return validationResult.Error;
        
        return new PetOrderNumber(number);
    }

    public static UnitResult<Error> Validate(int value)
    {
        if (value < Constants.Pet.MinOrderNumber)
            return Errors.General.ValueIsInvalid(nameof(PetOrderNumber),"Number must be more than zero");
        
        return UnitResult.Success<Error>();
    }

}