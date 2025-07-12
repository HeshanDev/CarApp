using CarApp.Application.Cars.Dtos;

using MediatR;

/// <summary>
/// Command to create a new car.
/// </summary>
public sealed record CreateCarCommand(
    string Brand,
    string Model,
    int Year,
    string Color
) : IRequest<CarDto>; // The result is a DTO of the created car
