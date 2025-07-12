using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using CarApp.Domain.ValueObjects;

public class CarIdJsonConverter : JsonConverter<CarId>
{
    public override CarId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var guid = reader.GetGuid();
        return CarId.FromGuid(guid);
    }

    public override void Write(Utf8JsonWriter writer, CarId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}
