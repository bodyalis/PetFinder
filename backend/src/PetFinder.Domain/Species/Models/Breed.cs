using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Species.ValueObjects;

namespace PetFinder.Domain.Species.Models;

public class Breed : SharedKernel.Entity<BreedId>
{
    private Breed(BreedId id)
        : base(id)
    {
    }

    private Breed(
        BreedId id,
        BreedTitle title,
        BreedDescription description, 
        SpeciesId speciesId) : base(id)
    {
        Title = title;
        Description = description;
        SpeciesId = speciesId;
    }

    public BreedDescription Description { get; private set; } = default!;
    public BreedTitle Title { get; private set; } = default!;
    public SpeciesId SpeciesId { get; private set; } = default!;

    public static Result<Breed, Error> Create(
        BreedId id,
        BreedTitle title,
        BreedDescription description,
        SpeciesId speciesId) 
        => new Breed(
            id: id,
            title: title,
            description: description,
            speciesId: speciesId);
}