using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PetFinder.Domain.Shared;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Application.Providers.IFileProvider;

public interface IFileProvider
{
    Task<UnitResult<UploadFileError>> UploadFile(FileContent fileContent, CancellationToken cancellationToken);
    
    IAsyncEnumerable<UnitResult<UploadFileError>> UploadFilesAsync(IEnumerable<FileContent> fileContents,
        CancellationToken cancellationToken);

    public Task<IEnumerable<UnitResult<UploadFileError>>> UploadFiles(IEnumerable<FileContent> fileContents,
        CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> RemoveFile(string fileName, string bucketName, CancellationToken cancellationToken);
    
    Task<Result<string, Error>> GetFile(string fileName, string bucketName, CancellationToken cancellationToken);
}

public class UploadFileError
{
    public UploadFileError(string fileName, Error error)
    {
        FileName = fileName;
        Error = error;
    }
    public string FileName { get; set; }
    public Error Error { get; set; }
}
