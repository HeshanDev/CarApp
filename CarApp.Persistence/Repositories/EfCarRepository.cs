using CarApp.Domain.Entities;
using CarApp.Domain.Repositories;
using CarApp.Domain.ValueObjects;
using CarApp.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

namespace CarApp.Persistence.Repositories;

/// <summary>
/// EF Core implementation of ICarRepository.
/// </summary>
public sealed class EfCarRepository : ICarRepository
{
    private readonly CarDbContext _dbContext;

    public EfCarRepository(CarDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Car car)
    {
        await _dbContext.Cars.AddAsync(car);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(CarId id)
    {
        var car = await _dbContext.Cars.FindAsync(id.Value);
        if (car != null)
        {
            _dbContext.Cars.Remove(car);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Car>> GetAllAsync()
    {
        return await _dbContext.Cars.ToListAsync();
    }

    public async Task<Car?> GetByIdAsync(CarId id)
    {
        return await _dbContext.Cars.FindAsync(id.Value);
    }

    public async Task UpdateAsync(Car car)
    {
        _dbContext.Cars.Update(car);
        await _dbContext.SaveChangesAsync();
    }
}
