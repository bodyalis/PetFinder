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

    public async Task<UnitResult<Error>> UploadFile(FileContent fileContent, CancellationToken cancellationToken)
    {
        logger.LogTrace("Starting upload file with FileName {FileName} BacketName: {BacketName}",
            fileContent.FileName, fileContent.BucketName);

        try
        {
            var args = new PutObjectArgs()
                .WithFileName(fileContent.FileName)
                .WithStreamData(fileContent.Stream)
                .WithObjectSize(fileContent.Stream.Length)
                .WithHeaders(fileContent.MetaData)
                .WithContentType(StreamContentType);
            _ = await client.PutObjectAsync(args, cancellationToken);

            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to upload file: {ex}", ex);

            return Error.Failure(ErrorCodes.FileUploadFailed, ex.Message);
        }
        finally
        {
            logger.LogTrace("Finished upload file with FileName {FileName} BacketName: {BacketName}",
                fileContent.FileName, fileContent.BucketName);
        }
    }

    public async IAsyncEnumerable<UnitResult<Error>> UploadFiles(IEnumerable<FileContent> fileContents,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        logger.LogTrace("Starting upload files");
        try
        {
            List<Task<UnitResult<Error>>> tasks = fileContents.Select(fileContent =>
                Task.Run(async () =>
                {
                    var checkBucketResult = await CreateBucketIfNotExists(fileContent.BucketName, cancellationToken);

                    return checkBucketResult.IsFailure
                        ? checkBucketResult
                        : await UploadFile(fileContent, cancellationToken);
                }, cancellationToken)).ToList();

            while (tasks.Count > 0)
            {
                var task = await Task.WhenAny(tasks);

                yield return await task;

                tasks.Remove(task);
            }
        }
        finally
        {
            logger.LogTrace("Finished upload files");
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