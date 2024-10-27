using CSharpFunctionalExtensions;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteers.ValueObjects;

public record SocialNetwork
{
    private SocialNetwork()
    {
    }

    private SocialNetwork(string title, string url)
    {
        Title = title;
        Url = url;
    }

    public string Title { get; } = default!;
    public string Url { get; } = default!;

    public static Result<SocialNetwork, Error> Create(string title, string url)
    {
        var validationResult = Validate(
            title,
            url);

        if (validationResult.IsFailure)
            return validationResult.Error;

        return new SocialNetwork(
            title,
            url);
    }

    public static UnitResult<Error> Validate(string title, string url)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > Constants.SocialNetwork.MaxTitleLength)
            return Errors.General.ValueIsInvalid(
                nameof(Title),
                StringHelper.GetValueEmptyOrMoreThanNeedString(Constants.SocialNetwork.MaxTitleLength));

        if (string.IsNullOrWhiteSpace(url) || title.Length > Constants.SocialNetwork.MaxUrlLength)
            return Errors.General.ValueIsInvalid(
                nameof(Url),
                StringHelper.GetValueEmptyOrMoreThanNeedString(Constants.SocialNetwork.MaxUrlLength));

        return UnitResult.Success<Error>();
    }
}