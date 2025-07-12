using System.Linq;

using CarApp.Application.Cars.Dtos;
using CarApp.Domain.Repositories;

using MediatR;

namespace CarApp.Application.Cars.Queries.Handlers;

/// <summary>
/// Handler for retrieving all cars.
/// </summary>
public class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, IEnumerable<CarDto>>
{
    private readonly ICarRepository _repository;

    public GetAllCarsQueryHandler(ICarRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CarDto>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
    {
        var cars = await _repository.GetAllAsync();
        return cars.Select(c => new CarDto(
            c.Id.Value,
            c.Brand,
            c.Model,
            c.Year,
            c.Color
        ));
    }
}
