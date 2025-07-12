namespace CarApp.Domain.ValueObjects;

/// <summary>
/// Strongly-typed ID for Car entity.
/// </summary>
/// 

//record is ideal for value objects: immutable and value-based equality.
public sealed class CarId
{
    public Guid Value { get; }

    private CarId(Guid value)
    {
        Value = value;
    }

    public static CarId New() => new(Guid.NewGuid());

    public static CarId From(Guid value) => new(value);

    public static CarId FromGuid(Guid id) => new CarId(id);

    // Optional: override equality, ToString(), etc.
}
