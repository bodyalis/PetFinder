using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFinder.Application.MessageQueues;
using PetFinder.Application.Providers.IFileProvider;
using FileInfo = PetFinder.Domain.Volunteers.ValueObjects.FileInfo;

namespace PetFinder.Infrastructure.Jobs;

public class MinioFilesCleanerJob(
    ILogger<MinioFilesCleanerJob> logger,
    IFileProvider fileProvider,
    IMessageQueue<IEnumerable<FileInfo>> queue)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting {job}", nameof(MinioFilesCleanerJob));

        while (!stoppingToken.IsCancellationRequested)
        {
            var fileInfoes = await queue.GetMessage(stoppingToken);

            foreach (var fileInfo in fileInfoes)
            {
                var result = await fileProvider.RemoveFile(fileInfo.Name, fileInfo.Path, stoppingToken);
                if (result.IsFailure)
                {
                    logger.LogError("Failed to remove file. BucketName {BucketName}, FileName {FileName}",
                        fileInfo.Path, fileInfo.Name);
                }
                else
                {
                    logger.LogInformation("Success to remove file: BucketName {BucketName}, FileName {FileName}",
                        fileInfo.Path, fileInfo.Name);
                }
            }
        }

        logger.LogInformation("Stopping {job}", nameof(MinioFilesCleanerJob));
    }
}