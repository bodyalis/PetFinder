using FluentAssertions;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteers.Enums;
using PetFinder.Domain.Volunteers.Models;
using PetFinder.Domain.Volunteers.ValueObjects;

namespace PetFinder.Domain.UnitTests;

public class VolunteerTests
{
    [Fact]
    public void Move_Pet_Should_Not_Affect_When_Pet_Already_At_This_Position()
    {
        // Arrange
        var volunteer = GetVolunteerWithPets(5);

        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];

        var firstPosition = PetOrderNumber.CreateFirst().Value;
        
        // Act
        var result = volunteer.MovePet(firstPet, firstPosition);
        
        // Assert
        result.IsSuccess.Should().BeTrue();

        volunteer.Pets[0].OrderNumber.Should().Be(firstPosition);
        volunteer.Pets[1].OrderNumber.Value.Should().Be(2);
        volunteer.Pets[2].OrderNumber.Value.Should().Be(3);
        volunteer.Pets[3].OrderNumber.Value.Should().Be(4);
        volunteer.Pets[4].OrderNumber.Value.Should().Be(5);
    }

    [Fact]
    public void Move_Pet_Than_Possible_Order_Number_Should_Return_Error()
    {
        // Arrage
        var volunteer = GetVolunteerWithPets(5);

        var firstPet = volunteer.Pets[0];

        // Act
        var sixthPosition = PetOrderNumber.Create(6).Value;

        var result = volunteer.MovePet(firstPet, sixthPosition);
        
        // Assert

        result.IsFailure.Should().BeTrue();

        result.Error.Message.Should().Be(Errors.General.ValueIsInvalid(nameof(PetOrderNumber),
            "The value exceeds the allowed value").Message);
    }

    [Fact]
    public void Move_Pet_To_Right_Side_Should_Be_Correct()
    {
        // Arrange
        var volunteer = GetVolunteerWithPets(5);
        
        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
        
        var thirdPosition = PetOrderNumber.Create(3).Value;
        
        // Act
        var result = volunteer.MovePet(firstPet, thirdPosition);

        // Assert
        result.IsSuccess.Should().BeTrue();

        secondPet.OrderNumber.Value.Should().Be(1);
        thirdPet.OrderNumber.Value.Should().Be(2);
        firstPet.OrderNumber.Value.Should().Be(3);
        fourthPet.OrderNumber.Value.Should().Be(4);
        fifthPet.OrderNumber.Value.Should().Be(5);
    }

    [Fact]
    public void Move_Pet_To_Left_Side_Should_Be_Correct()
    {
        // Arrange
        var volunteer = GetVolunteerWithPets(5);
        
        var firstPet = volunteer.Pets[0];
        var secondPet = volunteer.Pets[1];
        var thirdPet = volunteer.Pets[2];
        var fourthPet = volunteer.Pets[3];
        var fifthPet = volunteer.Pets[4];
        
        var firstPosition = PetOrderNumber.CreateFirst().Value;
        
        // Act
        var result = volunteer.MovePet(thirdPet, firstPosition);

        // Assert
        result.IsSuccess.Should().BeTrue();

        thirdPet.OrderNumber.Value.Should().Be(1);
        firstPet.OrderNumber.Value.Should().Be(2);
        secondPet.OrderNumber.Value.Should().Be(3);
        fourthPet.OrderNumber.Value.Should().Be(4);
        fifthPet.OrderNumber.Value.Should().Be(5);
    }

    private Volunteer GetVolunteerWithPets(int petsCount = 5)
    {
        var id = VolunteerId.New();
        var personName = PersonName.Create(
            "Bogdan",
            "Bogdanovich",
            "Bogdanov").Value;
        var phoneNumber = PhoneNumber.Create("+78005553535").Value;
        var email = Email.Create("bog@boga.net").Value;
        int experienceYears = 10;
        var description = VolunteerDescription.Create("Volunteer").Value;
        var socialNetworks = new ValueObjectList<SocialNetwork>
        (
            [
                SocialNetwork.Create("Facebook", "https://www.facebook.com").Value,
                SocialNetwork.Create("VK", "https://vk.com").Value
            ]
        );
        var assistanceDetails = new ValueObjectList<AssistanceDetails>
        (
            [
                AssistanceDetails.Create("Assistance", "Details").Value
            ]
        );

        var volunteer = Volunteer.Create(
            id,
            personName,
            phoneNumber,
            email,
            experienceYears,
            description,
            socialNetworks,
            assistanceDetails
        ).Value;

        for (int i = 0; i < petsCount; i++)
        {
            var pet = Pet.Create(
                PetId.New(),
                SpeciesBreedObject.Create(SpeciesId.New(), BreedId.New()).Value,
                PetName.Create($"Pet {i}").Value,
                PetGeneralDescription.Create($"Pet {i}").Value,
                PetColor.Create("Black").Value,
                PetHealthInformation.Create("Health").Value,
                Address.Create("Country", "City", "Street", "House", "Description").Value,
                100,
                100,
                PhoneNumber.Create("+78005553535").Value,
                DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
                true,
                true,
                HelpStatusPet.FoundHome,
                PetOrderNumber.Create(i + 1).Value
            ).Value;
            volunteer.AddPet(pet);
        }

        return volunteer;
    }
}