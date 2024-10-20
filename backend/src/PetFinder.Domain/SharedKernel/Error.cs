using PetFinder.Domain.Shared;

namespace PetFinder.Domain.SharedKernel;

public record Error
{
    // ReSharper disable once InconsistentNaming
    private const string Separator = "|||";

    public Error(
        string code,
        string message,
        ErrorType errorType,
        string? invalidField = null)
    {
        Code = code;
        Message = message;
        ErrorType = errorType;
        InvalidField = invalidField;
    }

    public string Code { get; }
    public string Message { get; }
    public ErrorType ErrorType { get; }
    public string? InvalidField { get; }

    public override string ToString()
        => $"Code: {Code}, Message: {Message}, ErrorType: {ErrorType}, InvalidField: {InvalidField}";

    public static implicit operator string(Error error) => error.ToString();

    
    public static Error Empty(string code, string message)
        => new(code, message, ErrorType.Empty);

    public static Error Validation(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Validation, invalidField);

    public static Error NotFound(string code, string message, string? invalidField = null)
        => new(code, message, ErrorType.NotFound, invalidField);

    public static Error Failure(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Failure, invalidField);

    public static Error Conflict(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Conflict, invalidField);

    public string Serialize()
        => string.Join(Separator, Code, Message, ErrorType, InvalidField);

    public static Error Deserialize(string serialized)
    {
        var parts = serialized.Split(Separator);
        if (parts.Length < 2)
            throw new InvalidOperationException("invalid serialize format");

        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
            throw new InvalidOperationException("invalid error type format");

        return new Error(parts[0], parts[1], type, parts.Length == 4 ? parts[3] : null);
    }

    public ErrorList ToErrorList()
    {
        return new ErrorList([this]);
    }
}

public enum ErrorType
{
    Empty,
    Validation,
    NotFound,
    Failure,
    Conflict
}