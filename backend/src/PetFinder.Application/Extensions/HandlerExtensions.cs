using Microsoft.Extensions.DependencyInjection;
using PetFinder.Application.Features.Shared.Interfaces;

namespace PetFinder.Application.Extensions;

internal static class HandlerExtensions
{
    public static IServiceCollection AddHandlersFromAssembly(this IServiceCollection services)
    {
        var interfaceType = typeof(IHandler);
        var types = typeof(IHandler).Assembly.GetTypes()
            .Where(p => interfaceType.IsAssignableFrom(p) && !p.IsInterface)
            .ToList();

        foreach (var type in types)
        {
            services.AddScoped(type);
        }

        return services;
    }
}