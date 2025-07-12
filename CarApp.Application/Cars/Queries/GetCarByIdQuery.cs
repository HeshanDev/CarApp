using CarApp.Application.Cars.Dtos;
using CarApp.Domain.ValueObjects;

using MediatR;

namespace CarApp.Application.Cars.Queries;

/// <summary>
/// Query to get a car by its ID.
/// </summary>
public sealed record GetCarByIdQuery(CarId Id) : IRequest<CarDto>;
