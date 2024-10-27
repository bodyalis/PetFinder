using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteers.ValueObjects;

public record PhoneNumber
{
    private PhoneNumber()
    {
    }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public string Value { get; } = default!;

    public static Result<PhoneNumber, Error> Create(string value)
    {
        var validationResult = Validate(value);

        if (validationResult.IsFailure)
            return validationResult.Error;

        return new PhoneNumber(value);
    }

    public static UnitResult<Error> Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !ValidationRegex.IsMatch(value))
            return Errors.General.ValueIsInvalid(nameof(PhoneNumber), $"is not match pattern {ValidationRegexPattern}");

        return UnitResult.Success<Error>();
    }

    private static readonly string ValidationRegexPattern = @"(^\+\d{1,3}\d{10}$|^$)";

    private static readonly Regex ValidationRegex = new Regex(
        pattern: ValidationRegexPattern,
        options: RegexOptions.Singleline | RegexOptions.Compiled);
}