using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Species.Models;

namespace PetFinder.Application.Features.Specles;

public interface ISpeciesRepository
{
    SpeciesId Add(Species species);
    Task<bool> ExistsByName(string name, CancellationToken cancellationToken);

    Task<Result<Species, Error>> GetById(SpeciesId id, CancellationToken cancellationToken);

    Task<bool> CheckExistsBreedBySpeciesIdAndBreedId(SpeciesId speciesId, BreedId breedId,
        CancellationToken cancellationToken);
}