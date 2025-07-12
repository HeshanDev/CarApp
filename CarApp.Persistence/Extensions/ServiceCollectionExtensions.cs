using CarApp.Domain.Repositories;
using CarApp.Persistence.Contexts;
using CarApp.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarApp.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<CarDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<ICarRepository, EfCarRepository>();

        return services;
    }
}
