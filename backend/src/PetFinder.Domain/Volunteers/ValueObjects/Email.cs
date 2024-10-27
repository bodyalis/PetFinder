using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteers.ValueObjects;

public record Email
{
    private Email()
    {
    }

    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; } = default!;

    public static Result<Email, Error> Create(string value)
    {
        var validationResult = Validate(value);

        if (validationResult.IsFailure)
            return validationResult.Error;

        return new Email(value: value);
    }

    public static UnitResult<Error> Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !ValidationRegex.IsMatch(value))
            return Errors.General.ValueIsInvalid(nameof(Email), $"is not match pattern {ValidationRegexPattern}");

        return UnitResult.Success<Error>();
    }

    private static readonly string ValidationRegexPattern = @"^[\w-\.]{1,40}@([\w-]+\.)+[\w-]{2,4}$";

    private static readonly Regex ValidationRegex = new Regex(
        pattern: ValidationRegexPattern,
        options: RegexOptions.Singleline | RegexOptions.Compiled);
}