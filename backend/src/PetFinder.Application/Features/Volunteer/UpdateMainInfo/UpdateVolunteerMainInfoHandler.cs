using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFinder.Application.DataLayer;
using PetFinder.Application.Extensions;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Domain.Shared;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteers.Models;
using PetFinder.Domain.Volunteers.ValueObjects;

namespace PetFinder.Application.Features.UpdateMainInfo;

// todo Need refactor for patch method for update only edited properties 
public class UpdateVolunteerMainInfoHandler(
    IVolunteerRepository volunteerRepository,
    IUnitOfWork unitOfWork,
    IValidator<UpdateVolunteerMainInfoCommand> validator,
    ILogger<UpdateVolunteerMainInfoHandler> logger) : ICommandHandler<UpdateVolunteerMainInfoCommand>
{
    public async Task<UnitResult<ErrorList>> Handle(
        UpdateVolunteerMainInfoCommand command,
        CancellationToken cancellationToken)
    {
        var dto = command.Dto;

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.Errors.ToErrorList();

        var volunteerResult = await volunteerRepository.GetById(VolunteerId.Create(command.Id), cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.RecordNotFound(nameof(Volunteer), command.Id).ToErrorList();

        var phoneNumber = PhoneNumber.Create(dto.PhoneNumber).Value;
        if (await volunteerRepository.CheckPhoneNumberForExists(phoneNumber, cancellationToken))
            return Errors.General.ValueIsNotUnique(nameof(PhoneNumber)).ToErrorList();

        var email = Email.Create(dto.Email).Value;
        if (await volunteerRepository.CheckEmailForExists(email, cancellationToken))
            return Errors.General.ValueIsNotUnique(nameof(Email)).ToErrorList();

        var personName = PersonName.Create(
            firstName: dto.PersonNameDto.FirstName,
            middleName: dto.PersonNameDto.MiddleName,
            lastName: dto.PersonNameDto.LastName).Value;

        var description = VolunteerDescription.Create(dto.VolunteerDescription).Value;

        var volunteer = volunteerResult.Value;
        volunteer.UpdateMainInfo(
            personName: personName,
            phoneNumber: phoneNumber,
            email: email,
            description: description,
            experienceYears: dto.ExperienceYears);

        volunteerRepository.Save(volunteer);

        await unitOfWork.SaveChanges(cancellationToken);
        return UnitResult.Success<ErrorList>();
    }


    // public async Task<Result<Guid, ErrorList>> HandlePatch(
    //     UpdateVolunteerMainInfoRequest request,
    //     CancellationToken cancellationToken)
    // {
    //     logger.LogTrace("Starting Handle"); 
    //     
    //     var validationResult = await validator.ValidateAsync(request, cancellationToken);
    //     if (!validationResult.IsValid)
    //         return validationResult.Errors.ToErrorList();
    //
    //     var volunteerResult = await volunteerRepository.GetById(VolunteerId.Create(request.Id), cancellationToken);
    //     if (volunteerResult.IsFailure)
    //         return Errors.General.RecordNotFound(nameof(Volunteer), request.Id).ToErrorList();
    //
    //     var dto = request.Dto;
    //     Volunteer volunteer = volunteerResult.Value;
    //
    //     PhoneNumber? phoneNumber = default;
    //     {
    //         if (!string.IsNullOrEmpty(dto.PhoneNumber))
    //         {
    //             phoneNumber = PhoneNumber.Create(dto.PhoneNumber).Value;
    //
    //             if (await volunteerRepository.CheckPhoneNumberForExists(phoneNumber, cancellationToken))
    //                 return Errors.General.ValueIsNotUnique(nameof(PhoneNumber)).ToErrorList();
    //         }
    //     }
    //
    //     Email? email = default;
    //     {
    //         if (!string.IsNullOrEmpty(dto.Email))
    //         {
    //             email = Email.Create(dto.Email).Value;
    //
    //             if (await volunteerRepository.CheckEmailForExists(email, cancellationToken))
    //                 return Errors.General.ValueIsNotUnique(nameof(Email)).ToErrorList();
    //         }
    //     }
    //     
    //     PersonName? personName = default;
    //     {
    //         if (dto.PersonNameDto is not null)
    //         {
    //             var personNameDto = dto.PersonNameDto;
    //
    //             personName = PersonName.Create(
    //                 firstName: personNameDto.FirstName,
    //                 middleName: personNameDto.MiddleName,
    //                 lastName: personNameDto.LastName).Value;
    //         }
    //     }
    //
    //     VolunteerDescription? description = default;
    //     {
    //         if (!string.IsNullOrEmpty(dto.VolunteerDescription))
    //             description = VolunteerDescription.Create(dto.VolunteerDescription).Value;
    //     }
    //     
    //     volunteer.UpdateMainInfo(
    //         personName: personName,
    //         phoneNumber: phoneNumber,
    //         email: email,
    //         description: description,
    //         experienceYears: dto.ExperienceYears);
    //     
    //     volunteerRepository.Save(volunteer);
    //
    //     await volunteerRepository.SaveChanges(cancellationToken);
    //     
    //     logger.LogTrace("Volunteer has been updated");
    //     return volunteer.Id.Value;
    // }
}