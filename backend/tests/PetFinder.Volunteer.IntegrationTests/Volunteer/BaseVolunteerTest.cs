using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetFinder.Infrastructure.DbContexts;

namespace PetFinder.Volunteer.IntegrationTests;

public abstract class BaseVolunteerTest : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly IntegrationTestsWebFactory Factory;
    protected readonly IFixture Fixture;
    protected readonly ReadDbContext ReadDbContext;
    protected readonly IServiceScope ServiceScope;
    protected readonly WriteDbContext WriteDbContext;

    public BaseVolunteerTest(IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        ServiceScope = factory.Services.CreateScope();
        WriteDbContext = ServiceScope.ServiceProvider.GetRequiredService<WriteDbContext>();
        ReadDbContext = ServiceScope.ServiceProvider.GetRequiredService<ReadDbContext>();
        Fixture = new Fixture();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        ServiceScope.Dispose();
        return Task.CompletedTask;
    }
}