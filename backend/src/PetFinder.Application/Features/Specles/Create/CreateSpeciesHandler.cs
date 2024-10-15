using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFinder.Application.DataLayer;
using PetFinder.Application.Extensions;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Domain.Shared;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Species.Models;
using PetFinder.Domain.Species.ValueObjects;

namespace PetFinder.Application.Features.Specles.Create;

public class CreateSpeciesHandler(
    IValidator<CreateSpeciesCommand> validator,
    IUnitOfWork unitOfWork,
    ISpeciesRepository repository,
    ILogger<CreateSpeciesHandler> logger) : IHandler
{
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateSpeciesCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.Errors.ToErrorList();

        if (await repository.ExistsByName(command.Title, cancellationToken))
            return Errors.General.ValueIsNotUnique(command.Title).ToErrorList();

        var species = Species.Create(
            SpeciesId.New(),
            SpeciesTitle.Create(command.Title).Value
        ).Value;

        repository.Add(species);

        await unitOfWork.SaveChanges(cancellationToken);

        return species.Id.Value;
    }
}