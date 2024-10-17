using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetFinder.Application.DataLayer;
using PetFinder.Application.Features.Shared.Interfaces;
using PetFinder.Application.Providers.IFileProvider;
using PetFinder.Domain.Shared;

namespace PetFinder.Application.Features.AddPetPhotos;

public class AddPetPhotosHandler(
    IVolunteerRepository volunteerRepository,
    IFileProvider fileProvider,
    IUnitOfWork unitOfWork,
    ILogger<AddPetPhotosHandler> logger)
    : IHandler
{
    public async Task<UnitResult<ErrorList>> Handle(
        AddPetPhotosCommand command,
        CancellationToken cancellationToken)
    {
        
        
        return UnitResult.Success<ErrorList>();
    }
}

