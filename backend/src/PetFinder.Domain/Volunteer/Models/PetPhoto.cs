using CSharpFunctionalExtensions;
using PetFinder.Domain.Shared.Exceptions;
using PetFinder.Domain.Shared.Ids;
using PetFinder.Domain.Shared.Interfaces;
using PetFinder.Domain.SharedKernel;
using FileInfo = PetFinder.Domain.Volunteer.ValueObjects.FileInfo;

namespace PetFinder.Domain.Volunteer.Models;

public class PetPhoto :
    SharedKernel.Entity<PetPhotoId>,
    ISoftDeletable
{
    private PetPhoto(PetPhotoId id)
        : base(id)
    {
    }

    private PetPhoto(
        PetPhotoId id,
        FileInfo fileInfo,
        bool isMain) : base(id)
    {
        FileInfo = fileInfo;
        IsMain = isMain;
        IsDeleted = false;
        DeletedAt = null;
    }

    public FileInfo FileInfo { get; private set; } = default!;
    public bool IsMain { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public static Result<PetPhoto, Error> Create(
        PetPhotoId id,
        FileInfo fileInfo,
        bool isMain)
    {
        return new PetPhoto(
            id: id,
            fileInfo: fileInfo,
            isMain: isMain);
    }
    

    public void Activate()
    {
        EntityAlreadyActivatedException.ThrowIfActivated(!IsDeleted);

        IsDeleted = false;
        DeletedAt = null;
    }

    public void Deactivate(DateTime deletedAt)
    {
        EntityAlreadyDeletedException.ThrowIfDeleted(IsDeleted);

        IsDeleted = true;
        DeletedAt = deletedAt;
    }
}