using AutoFixture;
using PetFinder.Application.Dto;
using PetFinder.Application.Features;
using PetFinder.Application.Features.CreatePet;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteers.Enums;

namespace PetFinder.Volunteer.IntegrationTests;

public static class FixtureExtension
{
    private const string PhoneNumber = "+79999999999";
    private const string Email = "test@test.com";
    private const int ExperienceYears = 10;

    public static CreatePetCommand BuildCreatePetCommand(
        this IFixture fixture, Guid volunteerId, Guid speciesId, Guid breedId)
        => fixture.CustomizeCreatePetCommand(volunteerId, speciesId, breedId).Create<CreatePetCommand>();

    private static IFixture CustomizeCreatePetCommand(
        this IFixture fixture,
        Guid volunteerId,
        Guid speciesId,
        Guid breedId)
    {
        fixture.Customize<DateOnly>(o => o.FromFactory((DateTime dt) => DateOnly.FromDateTime(dt)));
        fixture.Customize<TimeOnly>(o => o.FromFactory((DateTime dt) => TimeOnly.FromDateTime(dt)));

        fixture.CustomizeAddressDto()
            .Customize<CreatePetCommand>(composer =>
            {
                return composer
                    .With(command => command.Name,
                        fixture.Create<string>().CustomSubstring(Constants.Pet.MaxNameLength))
                    .With(command => command.GeneralDescription,
                        fixture.Create<string>().CustomSubstring(Constants.Pet.MaxGeneralDescriptionLength))
                    .With(command => command.Color,
                        fixture.Create<string>().CustomSubstring(Constants.Pet.MaxColorLength))
                    .With(command => command.HealthInformation,
                        fixture.Create<string>().CustomSubstring(Constants.Pet.MaxHealthInformationLength))
                    .With(command => command.OwnerPhoneNumber,
                        "+79053699816")
                    .With(command => command.BirthDate, DateOnly.FromDateTime(DateTime.Now - TimeSpan.FromDays(1)))
                    .With(command => command.HelpStatus, nameof(HelpStatusPet.FoundHome))
                    .With(command => command.Height, Random.Shared.NextDouble() + Constants.Pet.MinHeightValue)
                    .With(command => command.Weight, Random.Shared.NextDouble() + Constants.Pet.MinWeightValue)
                    .With(command => command.VolunteerId, volunteerId)
                    .With(command => command.SpeciesId, speciesId)
                    .With(command => command.BreedId, breedId);
            });

        return fixture;
    }

    private static IFixture CustomizeAddressDto(this IFixture fixture)
    {
        fixture.Customize<AddressDto>(composer =>
        {
            return composer
                .With(dto => dto.Description,
                    fixture.Create<string>().CustomSubstring(Constants.Address.MaxDescriptionLength))
                .With(dto => dto.City, fixture.CreateString(Constants.Address.MaxCityLength))
                .With(dto => dto.Country, fixture.Create<string>().CustomSubstring(Constants.Address.MaxCountryLength))
                .With(dto => dto.House, fixture.Create<string>().CustomSubstring(Constants.Address.MaxHouseLength))
                .With(dto => dto.Street, fixture.Create<string>().CustomSubstring(Constants.Address.MaxStreetLength));
        });

        return fixture;
    }

    public static CreateVolunteerCommand BuildCreateVolunteerCommand(this IFixture fixture)
        => fixture.CustomizeCreateVolunteerCommand().Create<CreateVolunteerCommand>();

    private static IFixture CustomizeCreateVolunteerCommand(this IFixture fixture)
    {
        fixture.CustomizePersonNameDto()
            .CustomizeSocialNetworkDto()
            .CustomizeAssistanceDetailsDto()
            .Customize<CreateVolunteerCommand>(composer =>
            {
                return composer
                    .With(command => command.Email, Email)
                    .With(command => command.Description,
                        fixture.Create<string>().CustomSubstring(Constants.Volunteer.MaxDescriptionLength))
                    .With(command => command.PhoneNumber,
                        PhoneNumber)
                    .With(command => command.ExperienceYears, ExperienceYears);
            });

        return fixture;
    }

    private static IFixture CustomizeAssistanceDetailsDto(this IFixture fixture)
    {
        fixture.Customize<AssistanceDetailsDto>(
            composer =>
            {
                return composer
                    .With(p => p.Title,
                        fixture.Create<string>().CustomSubstring(Constants.AssistanceDetail.MaxTitleLength))
                    .With(p => p.Description,
                        fixture.Create<string>().CustomSubstring(Constants.AssistanceDetail.MaxDescriptionLength));
            });

        return fixture;
    }

    private static IFixture CustomizeSocialNetworkDto(this IFixture fixture)
    {
        fixture.Customize<SocialNetworkDto>(
            composer =>
            {
                return composer
                    .With(p => p.Title,
                        fixture.Create<string>().CustomSubstring(Constants.SocialNetwork.MaxTitleLength))
                    .With(p => p.Url,
                        fixture.Create<string>().CustomSubstring(Constants.SocialNetwork.MaxUrlLength));
            });

        return fixture;
    }

    private static IFixture CustomizePersonNameDto(this IFixture fixture)
    {
        fixture.Customize<PersonNameDto>(composer =>
        {
            return composer
                .With(p => p.FirstName,
                    fixture.Create<string>().CustomSubstring(Constants.Volunteer.MaxFirstNameLength))
                .With(p => p.LastName,
                    fixture.Create<string>().CustomSubstring(Constants.Volunteer.MaxLastNameLength))
                .With(p => p.MiddleName,
                    fixture.Create<string>().CustomSubstring(Constants.Volunteer.MaxMiddleNameLength));
        });

        return fixture;
    }

    public static UpdateVolunteerMainInfoDto BuildUpdateVolunteerMainInfoDto(this IFixture fixture)
        => fixture.CustomizeUpdateVolunteerCommandDto().Create<UpdateVolunteerMainInfoDto>();

    private static IFixture CustomizeUpdateVolunteerCommandDto(this IFixture fixture)
    {
        fixture
            .CustomizePersonNameDto()
            .Customize<UpdateVolunteerMainInfoDto>(composer =>
            {
                return composer
                    .With(command => command.VolunteerDescription,
                        fixture.CreateString(Constants.Volunteer.MaxDescriptionLength))
                    .With(command => command.PhoneNumber,
                        PhoneNumber)
                    .With(command => command.Email, Email)
                    .With(command => command.ExperienceYears, ExperienceYears);
            });

        return fixture;
    }

    private static string CreateString(this IFixture fixture, int maxLength)
        => fixture.Create<string>().CustomSubstring(maxLength);

    /// <summary> Дефолтный метод для обрезки строки, т.к. выкидывает исключение, если string.Length меньше maxLength </summary>
    /// <param name="str"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    private static string CustomSubstring(this string str, int maxLength)
    {
        if (str.Length < maxLength)
            maxLength = str.Length;

        return str.Substring(0, maxLength);
    }
}