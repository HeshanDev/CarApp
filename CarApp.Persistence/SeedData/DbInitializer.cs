using CarApp.Domain.Entities;
using CarApp.Persistence.Contexts;

namespace CarApp.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(CarDbContext context)
    {
        // Creates DB and schema if not exists, returns true if it was just created
        bool dbJustCreated = await context.Database.EnsureCreatedAsync();

        if (!dbJustCreated)
            return;

        var sampleCars = GenerateSampleCars(20); // ✅ 20 cars

        context.Cars.AddRange(sampleCars);
        await context.SaveChangesAsync();
    }

    private static List<Car> GenerateSampleCars(int count)
    {
        var brands = new[] { "Toyota", "Honda", "Ford", "BMW", "Audi", "Tesla" };
        var models = new[] { "Corolla", "Civic", "Mustang", "X5", "A4", "Model 3" };
        var colors = new[] { "Red", "Blue", "Black", "White", "Gray" };
        var random = new Random();

        var cars = new List<Car>();
        for (int i = 0; i < count; i++)
        {
            var brand = brands[random.Next(brands.Length)];
            var model = models[random.Next(models.Length)];
            var color = colors[random.Next(colors.Length)];
            var year = random.Next(2015, 2024);

            var car = Car.Create(brand, model, year, color);
            cars.Add(car);
        }

        return cars;
    }
}
