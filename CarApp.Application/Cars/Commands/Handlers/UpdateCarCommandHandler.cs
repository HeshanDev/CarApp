using CarApp.Domain.Repositories;
using CarApp.Domain.ValueObjects;

using MediatR;

namespace CarApp.Application.Cars.Commands.Handlers;

/// <summary>
/// Handles updating a car.
/// </summary>
public class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand, Unit>
{
    private readonly ICarRepository _carRepository;

    public UpdateCarCommandHandler(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task<Unit> Handle(UpdateCarCommand request, CancellationToken cancellationToken)
    {
        var carId = CarId.FromGuid(request.Id);
        var existingCar = await _carRepository.GetByIdAsync(carId);

        if (existingCar is null)
        {
            throw new Exception("Car not found.");
        }

        existingCar.UpdateDetails(request.Brand, request.Model, request.Year, request.Color);

        await _carRepository.UpdateAsync(existingCar);

        return Unit.Value;
    }
}