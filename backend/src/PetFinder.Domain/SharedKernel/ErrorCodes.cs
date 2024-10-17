namespace PetFinder.Domain.SharedKernel;

public static class ErrorCodes
{
    public const string InternalServerError = "internal.server";
    public const string ValueIsInvalid = "value.is.invalid";
    public const string ValueIsRequired = "value.is.null";
    public const string RecordNotFound = "record.not.found";
    public const string RecordIsExists = "record.is.exists";
    public const string ValueIsNotUnique = "value.is.not.unique";
    public const string FileUploadFailed = "file.upload";
    public const string FileRemoveFailed = "file.remove";
    public const string FileGetFailed = "file.get";
    public const string InvalidEnum = "invalid.enum";
}