using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Species.ValueObjects;

namespace PetFinder.Domain.Species.Models;

public class Species : SharedKernel.Entity<SpeciesId>
{
    private readonly List<Breed> _breeds = default!;

    private Species(SpeciesId id)
        : base(id)
    {
    }

    private Species(
        SpeciesId id,
        SpeciesTitle title,
        IEnumerable<Breed>? breeds) : base(id) 
    {
        Title = title;
        _breeds = breeds?.ToList() ?? [];
    }

    public SpeciesTitle Title { get; private set; } = default!;
    public IReadOnlyList<Breed> Breeds => _breeds;

    public static Result<Species, Error> Create(
        SpeciesId id,
        SpeciesTitle title,
        IEnumerable<Breed>? breeds = null) 
        => new Species(
            id: id,
            title: title,
            breeds: breeds);

    public UnitResult<Error> AddBreed(Breed breed)
    {
        // todo Add StringComparison.IngnoreCase AnyExtension and use it here
        if (_breeds.Any(b => b.Title == breed.Title))
            return Errors.General.ValueIsNotUnique(nameof(Breed.Title));
        
        _breeds.Add(breed);
        
        return UnitResult.Success<Error>();
    }
}