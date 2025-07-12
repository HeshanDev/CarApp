using CarApp.Application.Cars.Dtos;
using CarApp.Domain.Entities;
using CarApp.Domain.Repositories;
using CarApp.Domain.ValueObjects;

using MediatR;

namespace CarApp.Application.Cars.Commands.Handlers;

/// <summary>
/// Handles CreateCarCommand and persists the car.
/// </summary>
public sealed class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, CarDto>
{
    private readonly ICarRepository _repository;

    public CreateCarCommandHandler(ICarRepository repository)
    {
        _repository = repository;
    }

    public async Task<CarDto> Handle(CreateCarCommand request, CancellationToken cancellationToken)
    {
        // Generate a new CarId value object
        var id = CarId.New();

        // Create the domain entity
        var car = new Car(id, request.Brand, request.Model, request.Year, request.Color);

        // Persist to repository
        await _repository.AddAsync(car);

        // Return as DTO
        return new CarDto(car.Id.Value, car.Brand, car.Model, car.Year, car.Color);
    }
}
