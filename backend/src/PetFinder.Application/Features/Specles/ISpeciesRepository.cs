using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.Species.Models;

namespace PetFinder.Application.Features.Specles;

public interface ISpeciesRepository
{
    SpeciesId Add(Species species);
    Task<bool> ExistsByName(string name, CancellationToken cancellationToken);
    
    Task<Result<Species>> GetById(SpeciesId id, CancellationToken cancellationToken);
    
}