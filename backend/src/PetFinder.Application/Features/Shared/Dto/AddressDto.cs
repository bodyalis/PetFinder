using CSharpFunctionalExtensions;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteer.ValueObjects;

namespace PetFinder.Application.Features.CreatePet;

public record AddressDto(string Country, string City, string Street, string House, string? Description)
{
    public Result<Address, Error> ToValueObject() => Address.Create(Country, City, Street, House, Description);
}