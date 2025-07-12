namespace CarApp.Application.Cars.Dtos;

/// <summary>
/// Data transfer object for Car.
/// </summary>
public sealed record CarDto(Guid Id, string Brand, string Model, int Year, string Color);
