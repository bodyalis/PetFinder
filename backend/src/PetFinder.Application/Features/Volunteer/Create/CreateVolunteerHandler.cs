using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFinder.Application.DataLayer;
using PetFinder.Application.Extensions;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Domain.Shared;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.Shared.ValueObjects;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteers.Models;
using PetFinder.Domain.Volunteers.ValueObjects;

namespace PetFinder.Application.Features;

public class CreateVolunteerHandler(
    IVolunteerRepository volunteerRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateVolunteerHandler> logger,
    IValidator<CreateVolunteerCommand> validator) : ICommandHandlerWithResponse<CreateVolunteerCommand, Guid>
{
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.Errors.ToErrorList();

        var email = Email.Create(command.Email).Value;

        if (await volunteerRepository.CheckEmailForExists(email, cancellationToken))
            return Errors.General.RecordWithValueIsNotUnique(
                nameof(Volunteer), nameof(Email), email.Value).ToErrorList();

        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        if (await volunteerRepository.CheckPhoneNumberForExists(phoneNumber, cancellationToken))
            return Errors.General.RecordWithValueIsNotUnique(
                nameof(Constants.Volunteer), nameof(PhoneNumber), phoneNumber.Value).ToErrorList();

        var personName = PersonName.Create(
            command.PersonNameDto.FirstName,
            command.PersonNameDto.MiddleName,
            command.PersonNameDto.LastName).Value;

        var description = VolunteerDescription.Create(command.Description).Value;

        IEnumerable<SocialNetwork> socialNetworks = command.SocialNetworkDtos
            .Select(dto => SocialNetwork.Create(dto.Title, dto.Url).Value).ToList();

        IEnumerable<AssistanceDetails> assistanceDetails = command.AssistanceDetailsDtos
            .Select(dto => AssistanceDetails.Create(dto.Title, dto.Description).Value).ToList();

        var volunteerId = VolunteerId.New();

        var createVolunteerResult = Volunteer.Create(
            id: volunteerId,
            personName: personName,
            phoneNumber: phoneNumber,
            email: email,
            socialNetworks: new ValueObjectList<SocialNetwork>(socialNetworks),
            assistanceDetails: new ValueObjectList<AssistanceDetails>(assistanceDetails),
            experienceYears: command.ExperienceYears,
            description: description);

        if (createVolunteerResult.IsFailure)
            return createVolunteerResult.Error.ToErrorList();

        volunteerRepository.Add(createVolunteerResult.Value);

        await unitOfWork.SaveChanges(cancellationToken);

        logger.LogInformation("Volunteer with {id} created successfully.", volunteerId.Value);

        return volunteerId.Value;
    }
}