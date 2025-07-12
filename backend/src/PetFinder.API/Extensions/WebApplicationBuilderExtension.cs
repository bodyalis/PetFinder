using PetFinder.Application;
using PetFinder.Infrastructure;

namespace PetFinder.API.Extensions;

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
    {
        builder.ConfigureServices();

        return builder;
    }

    private static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddEnvironmentVariables();
        
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.ConfigureLogging(builder.Configuration);
        
        builder.Services.AddHttpLogging(configureOptions =>
        {
            configureOptions.CombineLogs = true;
        });

        builder.Services
            .AddInfrastructure(builder.Configuration)
            .AddApplication();

        return builder;
    }
    
}