using System.Collections;
using System.Runtime.CompilerServices;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using PetFinder.Application.Providers.IFileProvider;
using PetFinder.Domain.SharedKernel;

namespace PetFinder.Infrastructure.Providers;

internal class MinioProvider(IMinioClient client, ILogger<MinioProvider> logger) : IFileProvider
{
    private const string StreamContentType = "application/octet-stream";

    private static readonly SemaphoreSlim CreateBucketSemaphore = new(1);
    public async Task<UnitResult<UploadFileError>> UploadFile(FileContent fileContent,
        CancellationToken cancellationToken)
    {
        try
        {
            var checkBucketResult = await CreateBucketIfNotExists(fileContent.BucketName, cancellationToken);
            if (checkBucketResult.IsFailure)
                return UnitResult.Failure(new UploadFileError(fileContent.FileName, checkBucketResult.Error));

            var args = new PutObjectArgs()
                .WithBucket(fileContent.BucketName)
                .WithObject(fileContent.FileName)
                .WithStreamData(fileContent.Stream)
                .WithObjectSize(fileContent.Stream.Length)
                .WithHeaders(fileContent.MetaData)
                .WithContentType(StreamContentType);
            _ = await client.PutObjectAsync(args, cancellationToken);

            return UnitResult.Success<UploadFileError>();
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to upload file: {ex}", ex);

            return new UploadFileError(fileContent.FileName,
                Error.Failure(ErrorCodes.FileUploadFailedInternal, ex.Message));
        }
    }

    public async Task<IEnumerable<UnitResult<UploadFileError>>> UploadFiles(IEnumerable<FileContent> fileContents,
        CancellationToken cancellationToken)
    {
        List<Task<UnitResult<UploadFileError>>> tasks = fileContents.Select(fileContent =>
            Task.Run(async () => await UploadFile(fileContent, cancellationToken), cancellationToken)).ToList();
        
        return  await Task.WhenAll(tasks);
    }

    public async IAsyncEnumerable<UnitResult<UploadFileError>> UploadFilesAsync(IEnumerable<FileContent> fileContents,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        List<Task<UnitResult<UploadFileError>>> tasks = fileContents.Select(fileContent =>
            Task.Run(async () =>
            {
                var checkBucketResult = await CreateBucketIfNotExists(fileContent.BucketName, cancellationToken);
                if (checkBucketResult.IsFailure)
                    return UnitResult.Failure(new UploadFileError(fileContent.FileName, checkBucketResult.Error));

                var uploadResult = await UploadFile(fileContent, cancellationToken);

                return uploadResult.IsFailure
                    ? uploadResult.Error
                    : UnitResult.Success<UploadFileError>();
            }, cancellationToken)).ToList();

        while (tasks.Count > 0)
        {
            var task = await Task.WhenAny(tasks);

            yield return await task;

            tasks.Remove(task);
        }
    }


    public async Task<UnitResult<Error>> RemoveFile(string fileName, string bucketName,
        CancellationToken cancellationToken)
    {
        logger.LogTrace("Starting to remove file: BucketName {bucketName}, FileName {fileName}", bucketName, fileName);
        try
        {
            var removeArgs = new RemoveObjectArgs().WithBucket(bucketName).WithObject(fileName);

            await client.RemoveObjectAsync(removeArgs, cancellationToken);

            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to remove file. BucketName {bucketName}, FileName {fileName}. Exception {ex}",
                bucketName, fileName, ex);

            return Error.Failure(ErrorCodes.FileRemoveFailed, $"Failed to remove bucket");
        }
        finally
        {
            logger.LogTrace("Finished to remove file: BucketName {bucketName}, FileName {fileName}",
                bucketName, fileName);
        }
    }

    public async Task<Result<string, Error>> GetFile(string fileName, string bucketName,
        CancellationToken cancellationToken)
    {
        logger.LogTrace("Starting to get file: BucketName {bucketName}, FileName {fileName}", bucketName, fileName);
        try
        {
            var getArgs = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(fileName);

            string url = await client.PresignedGetObjectAsync(getArgs);

            logger.LogTrace("Success to get file: BucketName {bucketName}, FileName {fileName}, Url {url}",
                bucketName, fileName, url);

            return url;
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to get file. BucketName {bucketName}, FileName {fileName}. Exception {ex}",
                bucketName, fileName, ex);

            return Error.Failure(ErrorCodes.FileGetFailed, $"Failed to get file");
        }
        finally
        {
            logger.LogTrace("Finished to get file: BucketName {bucketName}, FileName {fileName}",
                bucketName, fileName);
        }
    }

    private async Task<UnitResult<Error>> CreateBucketIfNotExists(string bucketName,
        CancellationToken cancellationToken)
    {
        try
        {
            var existsArgs = new BucketExistsArgs().WithBucket(bucketName);

            if (await client.BucketExistsAsync(existsArgs, cancellationToken))
                return UnitResult.Success<Error>();

            try
            {
                await CreateBucketSemaphore.WaitAsync(cancellationToken);

                if (await client.BucketExistsAsync(existsArgs, cancellationToken))
                    return UnitResult.Success<Error>();

                var makeArgs = new MakeBucketArgs().WithBucket(bucketName);

                await client.MakeBucketAsync(makeArgs, cancellationToken);

                return UnitResult.Success<Error>();
            }
            finally
            {
                CreateBucketSemaphore.Release();
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Exception: {ex}", ex);
            return Error.Failure(ErrorCodes.InternalServerError,
                "Failed to check exists or create bucket");
        }
    }
}