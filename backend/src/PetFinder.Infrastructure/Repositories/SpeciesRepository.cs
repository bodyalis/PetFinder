using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFinder.Application.Features.Specles;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Species.Models;

namespace PetFinder.Infrastructure.Repositories;

public class SpeciesRepository(ApplicationDbContext dbContext)
    : ISpeciesRepository
{
    public SpeciesId Add(Species species)
    {
        dbContext.Species.Add(species);
        return species.Id;
    }

    public async Task<bool> ExistsByName(string name, CancellationToken cancellationToken)
        => await dbContext.Species.AnyAsync(s => s.Title.Value == name, cancellationToken);

    public async Task<Result<Species, Error>> GetById(SpeciesId id, CancellationToken cancellationToken)
    {
        var species = await dbContext.Species
            .Include(s => s.Breeds)
            .SingleOrDefaultAsync(s => s.Id == id, cancellationToken);

        return species is null
            ? Errors.General.RecordNotFound(nameof(Species), nameof(SpeciesId))
            : species;
    }

    public async Task<bool> CheckExistsBreedBySpeciesIdAndBreedId(SpeciesId speciesId, BreedId breedId,
        CancellationToken cancellationToken)
    {
        return await dbContext.Species
            .AnyAsync(
                s => s.Id == speciesId && s.Breeds.Any(b => b.Id == breedId), 
                cancellationToken);
    }
}