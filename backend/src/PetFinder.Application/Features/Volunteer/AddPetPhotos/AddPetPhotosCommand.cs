using PetFinder.Application.Features.Shared;
using PetFinder.Application.Providers.IFileProvider;

namespace PetFinder.Application.Features.AddPetPhotos;

public record AddPetPhotosCommand(Guid VolunteerId, Guid PetId, IEnumerable<PetPhotoFileDto> Photos);