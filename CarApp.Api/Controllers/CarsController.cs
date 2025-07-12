using CarApp.Application.Cars.Commands;
using CarApp.Application.Cars.Dtos;
using CarApp.Application.Cars.Queries;
using CarApp.Domain.ValueObjects;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CarApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CarsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all cars.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CarDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllCarsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Get a single car by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CarDto>> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetCarByIdQuery(CarId.FromGuid(id)));

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Create a new car.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<CarDto>> Create(CreateCarCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Update a car.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateCarCommand command)
    {
        if (id != command.Id)
            return BadRequest("Mismatched ID");

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Delete a car.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteCarCommand(id));
        return NoContent();
    }
}
