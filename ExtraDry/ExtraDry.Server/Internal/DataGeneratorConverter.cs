using ExtraDry.Server.DataWarehouse;

namespace ExtraDry.Server.Internal;

public class DataGeneratorConverter : JsonConverter<IDataGenerator> {

    public override IDataGenerator Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IDataGenerator value, JsonSerializerOptions options)
    {
        var type = value.GetType();
        JsonSerializer.Serialize(writer, value, type, options);
    }
}
