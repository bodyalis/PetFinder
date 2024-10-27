using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFinder.Application.DataLayer;
using PetFinder.Application.Extensions;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Application.Providers.IFileProvider;
using PetFinder.Domain.Shared;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteers.Models;
using FileInfo = PetFinder.Domain.Volunteers.ValueObjects.FileInfo;

namespace PetFinder.Application.Features.AddPetPhotos;

public class AddPetPhotosHandler(
    IVolunteerRepository volunteerRepository,
    IFileProvider fileProvider,
    IUnitOfWork unitOfWork,
    IValidator<AddPetPhotosCommand> validator,
    ILogger<AddPetPhotosHandler> logger)
    : IHandler
{
    public async Task<UnitResult<ErrorList>> Handle(
        AddPetPhotosCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.Errors.ToErrorList();

        var volunteerResult = await volunteerRepository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var volunteer = volunteerResult.Value;
        var pet = volunteer.Pets.FirstOrDefault(p => p.Id == PetId.Create(command.PetId));
        if (pet is null)
            return Errors.General.RecordNotFound(nameof(Pet), command.PetId).ToErrorList();

        var fileContents = command.Photos.Select(p => new FileContent(
            Stream: p.Content,
            Guid.NewGuid() + Path.GetExtension(p.FileName),
            Constants.PetPhoto.BucketName)).ToList();

        var uploadResult = await fileProvider.UploadFiles(fileContents, cancellationToken);

        var unitResults = uploadResult.ToArray();

        if (unitResults.Any(r => r.IsFailure))
            return new ErrorList(unitResults
                .Where(r => r.IsFailure)
                .Select(r => r.Error.Error).ToList());

        var petPhotos = fileContents.Select(
            f => PetPhoto.Create(
                    id: PetPhotoId.New(),
                    fileInfo: FileInfo.Create(Constants.PetPhoto.BucketName, f.FileName).Value,
                    isMain: false)
                .Value).ToList();

        pet.AddPhotos(petPhotos);

        await unitOfWork.SaveChanges(cancellationToken);

        return UnitResult.Success<ErrorList>();
    }

    // todo Переписать на возможность частичной загрузки файлов c возвратом ошибочных - HTTP 207
    // public async Task<UnitResult<ErrorList>> Handle(
    //     AddPetPhotosCommand command,
    //     CancellationToken cancellationToken)
    // {
    //     var validationResult = await validator.ValidateAsync(command, cancellationToken);
    //     if (!validationResult.IsValid)
    //         return validationResult.Errors.ToErrorList();
    //
    //     var volunteerResult = await volunteerRepository
    //         .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
    //     if (volunteerResult.IsFailure)
    //         return volunteerResult.Error.ToErrorList();
    //
    //     var volunteer = volunteerResult.Value;
    //     var pet = volunteer.Pets.FirstOrDefault(p => p.Id == PetId.Create(command.PetId));
    //     if (pet is null)
    //         return Errors.General.RecordNotFound(nameof(Pet), command.PetId).ToErrorList();
    //
    //     var errorList = new ErrorList();
    //     foreach (var photoDto in command.Photos)
    //     {
    //         string extension = Path.GetExtension(photoDto.FileName);
    //         string fileName = Guid.NewGuid() + extension;
    //
    //         var fileContent = new FileContent(
    //             photoDto.Content,
    //             fileName,
    //             BucketName);
    //
    //         var uploadFileResult = await fileProvider.UploadFile(fileContent, cancellationToken);
    //         if (uploadFileResult.IsFailure)
    //         {
    //             var error = uploadFileResult.Error.Error;
    //             Error enrichmentFileNameError = new Error(
    //                 error.Code,
    //                 error.Message,
    //                 error.ErrorType,
    //                 uploadFileResult.Error.FileName);
    //
    //             errorList.Add(error);
    //         }
    //         else
    //         {
    //             FileInfo fileInfo = FileInfo.Create(BucketName, fileName).Value;
    //             PetPhoto photo = PetPhoto.Create(PetPhotoId.New(), fileInfo, false).Value;
    //
    //             pet.AddPhoto(photo);
    //         }
    //     }
    //
    //     if (errorList.Count() != command.Photos.Count())
    //         await unitOfWork.SaveChanges(cancellationToken);
    //
    //     if (errorList.Any())
    //         return UnitResult.Failure(errorList);
    //
    //     return UnitResult.Success<ErrorList>();
    // }
}