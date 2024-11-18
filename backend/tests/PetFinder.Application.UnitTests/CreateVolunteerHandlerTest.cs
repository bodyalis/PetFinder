using CSharpFunctionalExtensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetFinder.Application.DataLayer;
using PetFinder.Application.Dto;
using PetFinder.Application.Features;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.Volunteers.Models;
using PetFinder.Domain.Volunteers.ValueObjects;

namespace PetFinder.Application.UnitTests;

public class CreateVolunteerHandlerTest
{
    private readonly Mock<IVolunteerRepository> _volunteerRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<ILogger<CreateVolunteerHandler>> _logger = new();
    private readonly Mock<IValidator<CreateVolunteerCommand>> _validator = new();

    [Fact]
    public async Task Handle_Should_Create_New_Volunteer()
    {
        // Arrange
        var ct = new CancellationTokenSource().Token;
        
        _volunteerRepository
            .Setup(r => r.Add(It.IsAny<Volunteer>()))
            .Returns(VolunteerId.New());
        _volunteerRepository
            .Setup(r => r.CheckEmailForExists(It.IsAny<Email>(), ct))
            .ReturnsAsync(false);
        _volunteerRepository
            .Setup(r => r.CheckPhoneNumberForExists(It.IsAny<PhoneNumber>(), ct))
            .ReturnsAsync(false);
        
        _unitOfWork
            .Setup(u => u.SaveChanges(ct))
            .ReturnsAsync(1);
        
        _validator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateVolunteerCommand>(), ct))
            .ReturnsAsync(new ValidationResult());
        
        
        var handler = new CreateVolunteerHandler(
            _volunteerRepository.Object,
            _unitOfWork.Object,
            _logger.Object,
            _validator.Object
        );


        var personName = new PersonNameDto(
            "Bogdan",
            "Ivanov",
            "Ivanov");
        var socialNetworks = new List<SocialNetworkDto>();
        var assistanceDetails = new List<AssistanceDetailsDto>();
        
        var request = new CreateVolunteerCommand(
            personName,
            socialNetworks,
            assistanceDetails,
            "+79585401235",
            1,
            "Some description",
            "bog@boga.net");
        // Act

        var result = await handler.Handle(request, ct);
        
        // Assert
        
        Assert.True(result.IsSuccess);
    }
}