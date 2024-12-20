using PetFinder.Domain.Shared;

namespace PetFinder.API.Response;

public record Envelope
{
    public Envelope(object? result, ErrorList? errors)
    {
        Result = result;
        Errors = errors;
        TimeCreated = DateTime.Now;
    }

    public object? Result { get; }
    public ErrorList? Errors { get; init; }
    public DateTime TimeCreated { get; }

    public static Envelope Ok(object? result = null)
    {
        return new Envelope(result, null);
    }

    public static Envelope Error(ErrorList errors)
    {
        return new Envelope(null, errors);
    }
}