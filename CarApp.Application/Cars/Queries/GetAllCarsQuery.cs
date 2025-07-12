using CarApp.Application.Cars.Dtos;

using MediatR;

namespace CarApp.Application.Cars.Queries;

/// <summary>
/// Query to get all cars.
/// </summary>
public record GetAllCarsQuery : IRequest<IEnumerable<CarDto>>;
