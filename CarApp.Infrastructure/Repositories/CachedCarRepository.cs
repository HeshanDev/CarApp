using CarApp.Domain.Entities;
using CarApp.Domain.Interfaces;
using CarApp.Domain.Repositories;
using CarApp.Domain.ValueObjects;

namespace CarApp.Infrastructure.Repositories;

public class CachedCarRepository : ICarRepository
{
    private readonly ICarRepository _decorated;
    private readonly ICacheService _cacheService;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public CachedCarRepository(ICarRepository decorated, ICacheService cacheService)
    {
        _decorated = decorated;
        _cacheService = cacheService;
    }

    public async Task AddAsync(Car car)
    {
        await _decorated.AddAsync(car);
        var cacheKey = GetCacheKey(car.Id);
        await _cacheService.SetAsync(cacheKey, car, CacheDuration);
    }

    public async Task DeleteAsync(CarId id)
    {
        await _decorated.DeleteAsync(id);
        var cacheKey = GetCacheKey(id);
        await _cacheService.RemoveAsync(cacheKey);
    }

    public async Task<IEnumerable<Car>> GetAllAsync()
    {
        // For simplicity, do not cache GetAll - just forward
        return await _decorated.GetAllAsync();
    }

    public async Task<Car?> GetByIdAsync(CarId id)
    {
        var cacheKey = GetCacheKey(id);
        var cachedCar = await _cacheService.GetAsync<Car>(cacheKey);
        if (cachedCar != null)
            return cachedCar;

        var car = await _decorated.GetByIdAsync(id);
        if (car != null)
            await _cacheService.SetAsync(cacheKey, car, CacheDuration);

        return car;
    }

    public async Task UpdateAsync(Car car)
    {
        await _decorated.UpdateAsync(car);
        var cacheKey = GetCacheKey(car.Id);
        await _cacheService.SetAsync(cacheKey, car, CacheDuration);
    }

    private static string GetCacheKey(CarId id) => $"car:{id.Value}";
}
