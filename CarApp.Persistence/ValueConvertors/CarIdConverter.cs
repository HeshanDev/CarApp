using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CarApp.Domain.ValueObjects;

public class CarIdConverter : ValueConverter<CarId, Guid>
{
    public CarIdConverter() : base(
        id => id.Value,       // Convert CarId -> Guid for storage
        value => CarId.From(value)) // Convert Guid -> CarId when reading
    {
    }
}
