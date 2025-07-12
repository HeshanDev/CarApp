using CarApp.Domain.Shared;
using CarApp.Domain.ValueObjects;

namespace CarApp.Domain.Entities;

/// <summary>
/// Car aggregate root with basic properties.
/// </summary>
public sealed class Car : Entity<CarId>
{
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }
    public string Color { get; private set; }

    private Car() : base(default!) { } // EF Core only

    public Car(CarId id, string brand, string model, int year, string color)
        : base(id)
    {
        Brand = brand;
        Model = model;
        Year = year;
        Color = color;
    }

    public void UpdateDetails(string brand, string model, int year, string color)
    {
        Brand = brand;
        Model = model;
        Year = year;
        Color = color;
    }
}
