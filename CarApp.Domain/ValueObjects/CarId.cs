namespace CarApp.Domain.ValueObjects;

/// <summary>
/// Strongly-typed ID for Car entity.
/// </summary>
/// 

//record is ideal for value objects: immutable and value-based equality.
public sealed record CarId(Guid Value)
{
    public static CarId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}
