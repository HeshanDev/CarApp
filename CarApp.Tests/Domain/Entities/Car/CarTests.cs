using CarApp.Domain.Entities;

using Shouldly;

using Xunit;

namespace CarApp.Tests.Domain.Entities;

public class CarTests
{
    [Fact]
    public void Create_ShouldInitializeCarWithCorrectValues()
    {
        // Arrange
        var brand = "Toyota";
        var model = "Corolla";
        var year = 2020;
        var color = "Red";

        // Act
        var car = Car.Create(brand, model, year, color);

        // Assert
        car.ShouldNotBeNull();
        car.Id.ShouldNotBeNull(); // New CarId should be generated
        car.Brand.ShouldBe(brand);
        car.Model.ShouldBe(model);
        car.Year.ShouldBe(year);
        car.Color.ShouldBe(color);
    }

    [Fact]
    public void UpdateDetails_ShouldUpdatePropertiesCorrectly()
    {
        // Arrange
        var car = Car.Create("Honda", "Civic", 2018, "Black");

        var newBrand = "Tesla";
        var newModel = "Model 3";
        var newYear = 2023;
        var newColor = "White";

        // Act
        car.UpdateDetails(newBrand, newModel, newYear, newColor);

        // Assert
        car.Brand.ShouldBe(newBrand);
        car.Model.ShouldBe(newModel);
        car.Year.ShouldBe(newYear);
        car.Color.ShouldBe(newColor);
    }
}
