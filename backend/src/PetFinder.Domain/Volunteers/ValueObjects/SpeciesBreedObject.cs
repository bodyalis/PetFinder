using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteers.ValueObjects;

public record SpeciesBreedObject
{
    private SpeciesBreedObject()
    {
    }

    private SpeciesBreedObject(SpeciesId speciesId, BreedId breedId)
    {
        SpeciesId = speciesId;
        BreedId = breedId;  
    }

    public SpeciesId SpeciesId { get; } = default!;
    public BreedId BreedId { get; } = default!;

    public static Result<SpeciesBreedObject, Error> Create(SpeciesId speciesId, BreedId breedId)
        => new SpeciesBreedObject(
            speciesId: speciesId,
            breedId: breedId);
}