using CarApp.Application.Cars.Dtos;
using CarApp.Domain.ValueObjects;

using MediatR;

namespace CarApp.Application.Cars.Commands;

/// <summary>
/// Command to delete a car by ID.
/// </summary>
public record DeleteCarCommand(Guid Id) : IRequest<Unit>;
