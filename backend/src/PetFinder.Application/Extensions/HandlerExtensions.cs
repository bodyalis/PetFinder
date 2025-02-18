using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using PetFinder.Application.Features.Shared.Interfaces;

namespace PetFinder.Application.Extensions;

internal static class HandlerExtensions
{
    public static IServiceCollection AddHandlersFromAssembly(this IServiceCollection services)
    {
        var commandHandlerTypes = typeof(ICommandHandler<>).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { HandlerType = t, InterfaceType = i })
            .Where(x => x.InterfaceType.IsGenericType &&
                        x.InterfaceType.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
            .ToList();

        foreach (var handler in commandHandlerTypes)
        {
            services.AddScoped(handler.InterfaceType, handler.HandlerType);
        }

        // Регистрация всех ICommandHandlerWithResponse<in T, TResponse>
        var commandHandlerWithResponseTypes = typeof(ICommandHandlerWithResponse<,>).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { HandlerType = t, InterfaceType = i })
            .Where(x => x.InterfaceType.IsGenericType &&
                        x.InterfaceType.GetGenericTypeDefinition() == typeof(ICommandHandlerWithResponse<,>))
            .ToList();

        foreach (var handler in commandHandlerWithResponseTypes)
        {
            services.AddScoped(handler.InterfaceType, handler.HandlerType);
        }
        
        // Регистрация всех IQueryHandler<in T, TResponse>
        var queryHandlerTypes = typeof(IQueryHandler<,>).Assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { HandlerType = t, InterfaceType = i })
            .Where(x => x.InterfaceType.IsGenericType &&
                        x.InterfaceType.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
            .ToList();

        foreach (var handler in queryHandlerTypes)
        {
            services.AddScoped(handler.InterfaceType, handler.HandlerType);
        }

        return services;
    }
}