using CSharpFunctionalExtensions;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Domain.Volunteer.ValueObjects;

public record Address
{
    private Address()
    {
    }

    private Address(
        string country,
        string city,
        string street,
        string house,
        string? description)
    {
        Country = country;
        City = city;
        Street = street;
        House = house;
        Description = description;
    }

    public string Country { get; } = default!;
    public string City { get; } = default!;
    public string Street { get; } = default!;
    public string House { get; } = default!;
    public string? Description { get; private set; }

    
    public static Result<Address, Error> Create(string country, string city, string street, string house,
        string? description)
    {
        var validationResult = Validate(
            country: country,
            city: city,
            street: street,
            house: house,
            description: description);

        if (validationResult.IsFailure)
            return validationResult.Error;

        return new Address(
            country: country,
            city: city,
            street: street,
            house: house,
            description: description);
    }

    public static UnitResult<Error> Validate(string country, string city, string street, string house,
        string? description)
    {
        if (string.IsNullOrWhiteSpace(country) || country.Length > Constants.Address.MaxCountryLength)
            return Errors.General.ValueIsInvalid(
                nameof(Country),
                StringHelper.GetValueEmptyOrMoreThanNeedString(Constants.Address.MaxCountryLength));

        if (string.IsNullOrWhiteSpace(city) || city.Length > Constants.Address.MaxCityLength)
            return Errors.General.ValueIsInvalid(
                nameof(City),
                StringHelper.GetValueEmptyOrMoreThanNeedString(Constants.Address.MaxCityLength));

        if (string.IsNullOrWhiteSpace(street) || street.Length > Constants.Address.MaxStreetLength)
            return Errors.General.ValueIsInvalid(
                nameof(Street),
                StringHelper.GetValueEmptyOrMoreThanNeedString(Constants.Address.MaxStreetLength));

        if (string.IsNullOrWhiteSpace(house) || house.Length > Constants.Address.MaxStreetLength)
            return Errors.General.ValueIsInvalid(
                nameof(House),
                StringHelper.GetValueEmptyOrMoreThanNeedString(Constants.Address.MaxHouseLength));

        if (description?.Length > Constants.Address.MaxDescriptionLength)
            return Errors.General.ValueIsInvalid(
                nameof(Description),
                StringHelper.GetValueMoreThanNeedString(Constants.Address.MaxDescriptionLength));

        return UnitResult.Success<Error>();
    }
}