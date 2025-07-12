using Microsoft.Extensions.DependencyInjection;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteers.Enums;
using PetFinder.Domain.Volunteers.Models;
using PetFinder.Domain.Volunteers.ValueObjects;
using PetFinder.Infrastructure.DbContexts;
namespace PetFinder.Volunteer.IntegrationTests;

public class SeedManager
{
    private readonly WriteDbContext _writeDbContext;

    public SeedManager(IntegrationTestsWebFactory factory)
    {
        var scope = factory.Services.CreateScope();
        _writeDbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
    }

    public async Task<List<Domain.Volunteers.Models.Volunteer>> SeedVolunteers(int volunteerCountToSeed)
    {
        List<Domain.Volunteers.Models.Volunteer> volunteers = new List<Domain.Volunteers.Models.Volunteer>(volunteerCountToSeed);
        for (int i = 0; i < volunteerCountToSeed; i++)
        {

            var volunteer = Domain.Volunteers.Models.Volunteer.Create
            (
                id: VolunteerId.New(),
                personName: PersonName.Create(firstName: "test", middleName: "test", lastName: "test").Value,
                phoneNumber: PhoneNumber.Create("+79999999999").Value,
                email: Email.Create("test@test.com").Value,
                experienceYears: 10,
                description: VolunteerDescription.Create("test").Value,
                socialNetworks:
                new ValueObjectList<SocialNetwork>([SocialNetwork.Create("title", "https://url.url").Value]),
                assistanceDetails: new ValueObjectList<AssistanceDetails>([
                    AssistanceDetails.Create("title", "description").Value
                ])
            ).Value;
            volunteers.Add(volunteer);
        }
        
        await _writeDbContext.AddRangeAsync(volunteers, CancellationToken.None);
        await _writeDbContext.SaveChangesAsync(CancellationToken.None);
        return volunteers;
    }

    public async Task<List<Pet>> SeedPets(
        int count, 
        List<SpeciesBreedObject> speciesBreedObjects,
        List<Domain.Volunteers.Models.Volunteer> volunteers)
    {

        var resultList = new List<Pet>(count);
        
        for (int i = 0; i < count; i++)
        {

            PetId petId = PetId.New();
            SpeciesBreedObject speciesBreedObject = speciesBreedObjects[speciesBreedObjects.Count % i];
            Domain.Volunteers.Models.Volunteer volunteer = volunteers[volunteers.Count % i];
            PetName petName = PetName.Create($"test_{i}").Value;
            PetGeneralDescription petDescription = PetGeneralDescription.Create($"test_{i}").Value;
            PetColor petColor = PetColor.Create($"test_{i}").Value;
            PetHealthInformation healthInformation = PetHealthInformation.Create($"test_{i}").Value;
            Address address = Address.Create($"test_{i}", $"test_{i}", $"test_{i}", $"test_{i}", $"test_{i}").Value;
            double weight = 1;
            double height = 1;
            PhoneNumber ownerPhoneNumber = PhoneNumber.Create($"+79999999999").Value;
            DateOnly birthDate = DateOnly.FromDateTime(DateTime.Now);
            bool isCastrated = false;
            bool isVaccinated = false;
            HelpStatusPet helpStatusPet = HelpStatusPet.FoundHome;
            PetOrderNumber orderNumber = PetOrderNumber.CreateFirst().Value;
            Pet pet = Pet.Create(
                petId,
                speciesBreedObject,
                petName,
                petDescription,
                petColor,
                healthInformation,
                address,
                weight,
                height,
                ownerPhoneNumber,
                birthDate,
                isCastrated,
                isVaccinated,
                helpStatusPet,
                orderNumber).Value;

            volunteer.AddPet(pet);
            resultList.Add(pet);
        }
        
        _writeDbContext.UpdateRange(volunteers, CancellationToken.None);
        
        await _writeDbContext.SaveChangesAsync(CancellationToken.None);
        
        return resultList;
    }
}