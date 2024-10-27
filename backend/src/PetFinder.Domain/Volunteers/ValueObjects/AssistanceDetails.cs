using CSharpFunctionalExtensions;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteers.ValueObjects;

public record AssistanceDetails
{
    private AssistanceDetails()
    {
    }

    private AssistanceDetails(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public string Title { get; } = default!;
    public string Description { get; } = default!;

    public static Result<AssistanceDetails, Error> Create(string title, string description)
    {
        var validationResult = Validate(
            title: title,
            description: description);

        if (validationResult.IsFailure)
            return validationResult.Error;

        return new AssistanceDetails(
            description: description,
            title: title);
    }

    public static UnitResult<Error> Validate(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > Constants.AssistanceDetail.MaxTitleLength)
            return Errors.General.ValueIsInvalid(
                nameof(Title),
                StringHelper.GetValueEmptyOrMoreThanNeedString(Constants.AssistanceDetail.MaxTitleLength));

        if (string.IsNullOrWhiteSpace(description) ||
            description.Length > Constants.AssistanceDetail.MaxDescriptionLength)
            return Errors.General.ValueIsInvalid(
                nameof(Title),
                StringHelper.GetValueEmptyOrMoreThanNeedString(Constants.AssistanceDetail.MaxDescriptionLength));

        return UnitResult.Success<Error>();
    }
}