using System.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFinder.Application.Messaging;
using PetFinder.Infrastructure.Messaging;
using PetFinder.Infrastructure.Options;
using FileInfo = PetFinder.Domain.Volunteers.ValueObjects.FileInfo;

namespace PetFinder.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMinio(this IServiceCollection services,
        IConfiguration configuration)
    {
        var minioOptions = configuration.GetSection(MinioOptions.SectionName).Get<MinioOptions>()
            ?? throw new InvalidOperationException("No options for Minio");

        services.AddMinio(config =>
        {
            config.WithEndpoint(minioOptions.Endpoint);
            config.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            config.WithSSL(minioOptions.Ssl);

            // todo configure minio logging
            config.SetTraceOn();
        });

        return services;
    }
}