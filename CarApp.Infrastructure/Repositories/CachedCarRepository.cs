using CarApp.Domain.Entities;
using CarApp.Domain.Interfaces;
using CarApp.Domain.Repositories;
using CarApp.Domain.ValueObjects;

using Microsoft.Extensions.Logging;

namespace CarApp.Infrastructure.Repositories;

public class CachedCarRepository : ICarRepository
{
    private readonly ICarRepository _decorated;
    private readonly ICacheService _cacheService;
    private readonly ILogger<CachedCarRepository> _logger;

    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
    private const string AllCarsCacheKey = "car:all";

    public CachedCarRepository(
        ICarRepository decorated,
        ICacheService cacheService,
        ILogger<CachedCarRepository> logger)
    {
        _decorated = decorated;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<Car?> GetByIdAsync(CarId id)
    {
        var cacheKey = GetCacheKey(id);
        var cachedCar = await _cacheService.GetAsync<Car>(cacheKey);

        if (cachedCar != null)
        {
            _logger.LogInformation("Cache HIT for key {CacheKey}", cacheKey);
            return cachedCar;
        }

        _logger.LogInformation("Cache MISS for key {CacheKey}", cacheKey);

        var car = await _decorated.GetByIdAsync(id);
        if (car != null)
            await _cacheService.SetAsync(cacheKey, car, CacheDuration);

        return car;
    }

    public async Task<IEnumerable<Car>> GetAllAsync()
    {
        var cachedCars = await _cacheService.GetAsync<IEnumerable<Car>>(AllCarsCacheKey);
        if (cachedCars != null)
        {
            _logger.LogInformation("Cache HIT for all cars");
            return cachedCars;
        }

        _logger.LogInformation("Cache MISS for all cars");

        var cars = await _decorated.GetAllAsync();
        await _cacheService.SetAsync(AllCarsCacheKey, cars, CacheDuration);

        return cars;
    }

    public async Task AddAsync(Car car)
    {
        await _decorated.AddAsync(car);
        var cacheKey = GetCacheKey(car.Id);
        await _cacheService.SetAsync(cacheKey, car, CacheDuration);

        // Refresh cached list
        await RefreshAllCarsCacheAsync();
    }

    public async Task UpdateAsync(Car car)
    {
        await _decorated.UpdateAsync(car);
        var cacheKey = GetCacheKey(car.Id);
        await _cacheService.SetAsync(cacheKey, car, CacheDuration);

        // Refresh cached list
        await RefreshAllCarsCacheAsync();
    }

    public async Task DeleteAsync(CarId id)
    {
        await _decorated.DeleteAsync(id);
        var cacheKey = GetCacheKey(id);
        await _cacheService.RemoveAsync(cacheKey);

        // Refresh cached list
        await RefreshAllCarsCacheAsync();
    }

    private async Task RefreshAllCarsCacheAsync()
    {
        var cars = await _decorated.GetAllAsync();
        await _cacheService.SetAsync(AllCarsCacheKey, cars, CacheDuration);
    }

    private static string GetCacheKey(CarId id) => $"car:{id.Value}";
}
