using CarApp.Domain.Repositories;
using CarApp.Domain.ValueObjects;

using MediatR;

namespace CarApp.Application.Cars.Commands.Handlers;

/// <summary>
/// Handles deletion of a car.
/// </summary>
public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand, Unit>
{
    private readonly ICarRepository _carRepository;

    public DeleteCarCommandHandler(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task<Unit> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
    {
        var carId = CarId.FromGuid(request.Id);
        var car = await _carRepository.GetByIdAsync(carId);

        if (car is null)
        {
            throw new Exception("Car not found.");
        }

        await _carRepository.DeleteAsync(carId);

        return Unit.Value; // ✅ Required
    }
}
