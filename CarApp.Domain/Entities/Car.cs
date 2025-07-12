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

    // Private constructor for EF Core only
    private Car() : base(default!) { }

    // Public constructor (still valid if needed for some services)
    public Car(CarId id, string brand, string model, int year, string color)
        : base(id)
    {
        Brand = brand;
        Model = model;
        Year = year;
        Color = color;
    }

    /// <summary>
    /// Domain factory method for creating new Car instances with a new CarId.
    /// </summary>
    public static Car Create(string brand, string model, int year, string color)
    {
        var id = CarId.New(); // generates new GUID internally
        return new Car(id, brand, model, year, color);
    }

    public void UpdateDetails(string brand, string model, int year, string color)
    {
        Brand = brand;
        Model = model;
        Year = year;
        Color = color;
    }
}
