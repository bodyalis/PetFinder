using FluentValidation;
using PetFinder.Application.Extensions;
using PetFinder.Domain.Species.ValueObjects;

namespace PetFinder.Application.Features.Specles.Create;

public class CreateSpeciesCommandValidator : AbstractValidator<CreateSpeciesCommand>
{
    public CreateSpeciesCommandValidator()
    {
        RuleFor(command => command.Title).MustBeValueObject(SpeciesTitle.Validate);
    }
}