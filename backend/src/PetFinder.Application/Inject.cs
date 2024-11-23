using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFinder.Application.Extensions;

namespace PetFinder.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddHandlersFromAssembly()
            .AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }
}