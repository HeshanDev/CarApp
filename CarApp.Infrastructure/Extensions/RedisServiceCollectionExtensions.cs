using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarApp.Infrastructure.Extensions;

/// <summary>
/// Configures distributed caching using Redis.
/// </summary>
public static class RedisServiceCollectionExtensions
{
    public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            // Get Redis connection string from config: "Redis:Configuration"
            // options.Configuration = configuration["Redis:Configuration"] ?? "localhost:6379";
            options.Configuration = configuration["Redis:Configuration"];
        });

        return services;
    }
}
