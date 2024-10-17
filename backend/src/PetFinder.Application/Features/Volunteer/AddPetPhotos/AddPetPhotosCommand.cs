using PetFinder.Application.Features.Shared;

namespace PetFinder.Application.Features.AddPetPhotos;

public record AddPetPhotosCommand(Guid VolunteerId, Guid PetId, IEnumerable<FileDto> Files);