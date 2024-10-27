using FluentValidation;
using PetFinder.Application.Extensions;
using PetFinder.Domain.SharedKernel;
using PetFinder.Domain.Volunteers.Models;
using FileInfo = PetFinder.Domain.Volunteers.ValueObjects.FileInfo;

namespace PetFinder.Application.Features.AddPetPhotos;

public class AddPetPhotosCommandValidator : AbstractValidator<AddPetPhotosCommand>
{
    public AddPetPhotosCommandValidator()
    {
        const int maxPhotoSize = 5 * 1024 * 1024; // 10 MB

        RuleFor(command => command.Photos)
            .Must(p => p.Any())
            .WithError(Errors.General.ValueIsRequired(nameof(PetPhoto)));
        
        RuleFor(command => command.Photos)
            .ForEach(photoRuleBuilder =>
            {
                photoRuleBuilder
                    .MustBeValueObject(photo => FileInfo.ValidateName(photo.FileName));

                photoRuleBuilder
                    .Must(photo => photo.Content.Length > 0)
                    .WithError(Errors.File.ContentIsEmpty());

                photoRuleBuilder
                    .Must(photo => photo.Content.Length <= maxPhotoSize)
                    .WithError(Errors.File.ContentIsTooBig(maxPhotoSize));

            });
    }
}