using System.Reflection;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace CarApp.Api.Extensions;

/// <summary>
/// Extension methods for dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register application services like MediatR and others.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register MediatR from the Application assembly
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.Load("CarApp.Application"));
        });

        return services;
    }
}
