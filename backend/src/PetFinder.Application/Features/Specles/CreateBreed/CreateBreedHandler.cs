using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFinder.Application.DataLayer;
using PetFinder.Application.Extensions;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Domain.Shared;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Species.Models;
using PetFinder.Domain.Species.ValueObjects;

namespace PetFinder.Application.Features.Specles.CreateBreed;

public class CreateBreedHandler(
    ILogger<CreateBreedHandler> logger,
    ISpeciesRepository repository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateBreedCommand>
{
    public async Task<UnitResult<ErrorList>> Handle(
        CreateBreedCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await new CreateBreedCommandValidator().ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.Errors.ToErrorList();

        var speciesResult = await repository.GetById(SpeciesId.Create(command.SpeciesId), cancellationToken);
        if (speciesResult.IsFailure)
            return Errors.General.RecordNotFound(nameof(Species), command.SpeciesId).ToErrorList();

        var species = speciesResult.Value;
        var breed = Breed.Create(
            BreedId.New(),
            BreedTitle.Create(command.Title).Value,
            BreedDescription.Create(command.Description).Value,
            species.Id
        ).Value;

        var result = species.AddBreed(breed);
        if (result.IsFailure)
            return result.Error.ToErrorList();

        await unitOfWork.SaveChanges(cancellationToken);

        return UnitResult.Success<ErrorList>();
    }
}