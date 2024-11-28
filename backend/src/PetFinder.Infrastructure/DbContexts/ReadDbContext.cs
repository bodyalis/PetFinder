using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetFinder.Application.DataLayer;
using PetFinder.Application.Dto;
using PetFinder.Infrastructure.Dto;

namespace PetFinder.Infrastructure.DbContexts;

public class ReadDbContext : DbContext, IReadDbContext
{
    private readonly IConfiguration _configuration = null!;

    private ReadDbContext()
    {
    }

    public ReadDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<VolunteerDto> Volunteers => Set<VolunteerDto>();
    public DbSet<PetDto> Pets => Set<PetDto>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Database")
                                 ?? throw new InvalidOperationException("No connection string by Database"))
            .UseSnakeCaseNamingConvention()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .UseLoggerFactory(CreateLoggerFactory())
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            GetType().Assembly,
            type => type.FullName!.Contains("Configurations.Read"));
    }

    private static ILoggerFactory CreateLoggerFactory()
        => LoggerFactory.Create(builder => builder.AddConsole());
}