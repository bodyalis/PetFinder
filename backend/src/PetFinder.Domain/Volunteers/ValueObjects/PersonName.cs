using CSharpFunctionalExtensions;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteers.ValueObjects;

public record PersonName
{
    private PersonName()
    {
    }

    private PersonName(string firstName, string lastName, string? middleName)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }
    
    public string FirstName { get;  } = default!;
    public string? MiddleName { get; }
    public string LastName { get; } = default!;

    public static Result<PersonName, Error> Create(string firstName, string? middleName, string lastName)
    {
        var validationResult = Validate(
            firstName: firstName,
            middleName: middleName,
            lastName: lastName);

        if (validationResult.IsFailure)
            return validationResult.Error;

        return new PersonName(
            firstName: firstName,
            lastName: lastName,
            middleName: middleName);
    }

    public static UnitResult<Error> Validate(string firstName, string? middleName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > Constants.Volunteer.MaxFirstNameLength)
            return Errors.General.ValueIsInvalid(
                valueName: nameof(FirstName),
                description: StringHelper.GetValueEmptyOrMoreThanNeedString(Constants.Volunteer.MaxFirstNameLength));

        if (middleName?.Length > Constants.Volunteer.MaxMiddleNameLength)
            return Errors.General.ValueIsInvalid(
                    valueName: nameof(LastName),
                    description: StringHelper.GetValueEmptyOrMoreThanNeedString(Constants.Volunteer.MaxLastNameLength));

        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length > Constants.Volunteer.MaxLastNameLength)
            return Errors.General.ValueIsInvalid(
                valueName: nameof(MiddleName),
                description: StringHelper.GetValueEmptyOrMoreThanNeedString(Constants.Volunteer.MaxLastNameLength));

        return UnitResult.Success<Error>();
    }
}