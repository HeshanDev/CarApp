using CarApp.Application.Cars.Dtos;
using CarApp.Domain.Exceptions;
using CarApp.Domain.Repositories;

using MediatR;

namespace CarApp.Application.Cars.Queries.Handlers;

/// <summary>
/// Handles the GetCarById query.
/// </summary>
/// 

//IRequest<CarDto>: MediatR knows this query returns a CarDto
//Handler receives the query and handles it
//We throw a custom exception if the car isn’t found
public sealed class GetCarByIdQueryHandler : IRequestHandler<GetCarByIdQuery, CarDto>
{
    private readonly ICarRepository _repository;

    public GetCarByIdQueryHandler(ICarRepository repository)
    {
        _repository = repository;
    }

    public async Task<CarDto> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
    {
        // Try to get the car by ID from the repository
        var car = await _repository.GetByIdAsync(request.Id);

        if (car is null)
            throw new CarNotFoundException($"Car with ID {request.Id} not found");

        // Map domain entity to DTO
        return new CarDto(
            car.Id.Value,
            car.Brand,
            car.Model,
            car.Year,
            car.Color
        );
    }
}
