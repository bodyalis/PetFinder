using PetFinder.Application.Dto;
using PetFinder.Application.Features.Shared.Interfaces;

namespace PetFinder.Application.Features.AddPetPhotos;

public record AddPetPhotosCommand(Guid VolunteerId, Guid PetId, IEnumerable<PetPhotoFileDto> Photos) : ICommand;