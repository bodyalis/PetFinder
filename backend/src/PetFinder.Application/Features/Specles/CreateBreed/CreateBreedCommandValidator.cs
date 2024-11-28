using FluentValidation;
using PetFinder.Application.Extensions;
using PetFinder.Domain.Species.ValueObjects;

namespace PetFinder.Application.Features.Specles.CreateBreed;

internal class CreateBreedCommandValidator : AbstractValidator<CreateBreedCommand>
{
    public CreateBreedCommandValidator()
    {
        RuleFor(command => command.Title).MustBeValueObject(BreedTitle.Validate);
        RuleFor(command => command.Description).MustBeValueObject(BreedDescription.Validate);
    }
}