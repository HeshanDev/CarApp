using CarApp.Domain.Entities;

namespace CarApp.Domain.DomainEvents;

/// <summary>
/// Event raised when a car is created.
/// </summary>
public sealed record CarCreatedDomainEvent(Car Car);
