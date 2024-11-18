using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFinder.Application.DataLayer;
using PetFinder.Application.Features;
using PetFinder.Application.Features.Specles;
using PetFinder.Application.Messaging;
using PetFinder.Application.Providers;
using PetFinder.Application.Providers.IFileProvider;
using PetFinder.Infrastructure.Extensions;
using PetFinder.Infrastructure.Jobs;
using PetFinder.Infrastructure.Messaging;
using PetFinder.Infrastructure.Providers;
using PetFinder.Infrastructure.Repositories;
using FileInfo = PetFinder.Domain.Volunteers.ValueObjects.FileInfo;

namespace PetFinder.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, EnumerableFileInfoMessageQueue>();
        
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IFileProvider, MinioProvider>();
        
        services.ConfigureMinio(configuration);
        
        services.AddDbContext<ApplicationDbContext>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        
        services.AddHostedService<MinioFilesCleanerJob>();
        
        return services;
    }
}