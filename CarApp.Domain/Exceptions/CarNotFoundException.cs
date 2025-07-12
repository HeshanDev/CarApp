namespace CarApp.Domain.Exceptions;

/// <summary>
/// Thrown when a car is not found in the repository.
/// </summary>
public sealed class CarNotFoundException : Exception
{
    public CarNotFoundException(string message) : base(message) { }
}
