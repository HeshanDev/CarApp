using System.Reflection;

using CarApp.Domain.Interfaces;
using CarApp.Domain.Repositories;
using CarApp.Infrastructure.Repositories;
using CarApp.Infrastructure.Services;
using CarApp.Persistence.Repositories;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using StackExchange.Redis;

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

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string redisConnectionString)
    {
        // Register Redis connection multiplexer as singleton
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(redisConnectionString));

        // Register RedisCacheService
        services.AddScoped<ICacheService, RedisCacheService>();

        // Register the EF repository first
        services.AddScoped<EfCarRepository>();

        // Register ICarRepository and decorate it with CachedCarRepository using Scrutor
        services.AddScoped<ICarRepository>(sp => sp.GetRequiredService<EfCarRepository>());
        services.Decorate<ICarRepository, CachedCarRepository>();

        return services;
    }
}
