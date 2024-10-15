using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFinder.Application.Features.Specles;
using PetFinder.Domain.Shared.Ids;
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
        => await dbContext.Species
            .AnyAsync(s => s.Title.Value == name , cancellationToken);

    public async Task<Result<Species>> GetById(SpeciesId id, CancellationToken cancellationToken) 
        => await dbContext.Species
               .Include(s => s.Breeds)
               .SingleOrDefaultAsync(s => s.Id == id, cancellationToken) 
           ?? Result.Failure<Species>("Not found");
}