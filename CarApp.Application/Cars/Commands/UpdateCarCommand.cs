using CarApp.Application.Cars.Dtos;
using CarApp.Domain.ValueObjects;

using MediatR;

namespace CarApp.Application.Cars.Commands;

/// <summary>
/// Command to update an existing car.
/// </summary>
public class UpdateCarCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
    public string Brand { get; init; } = default!;
    public string Model { get; init; } = default!;
    public string Color { get; init; } = default!;
    public int Year { get; init; }
}
