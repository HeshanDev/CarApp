using CarApp.Domain.Entities;
using CarApp.Domain.ValueObjects;

namespace CarApp.Domain.Repositories;

/// <summary>
/// Abstraction for Car persistence operations.
/// </summary>
public interface ICarRepository
{
    Task<Car?> GetByIdAsync(CarId id);
    Task<IEnumerable<Car>> GetAllAsync();
    Task AddAsync(Car car);
    Task UpdateAsync(Car car);
    Task DeleteAsync(CarId id);
}
