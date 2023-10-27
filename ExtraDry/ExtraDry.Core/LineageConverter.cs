using System.Text.Json;

namespace ExtraDry.Core;

/// <summary>
/// A JSON converter that will serialize a Lineage as the standard string representation.
/// </summary>
public class LineageConverter : JsonConverter<Lineage> {

    /// <inheritdoc cref="JsonConverter{T}.ReadAsPropertyName(ref Utf8JsonReader, Type, JsonSerializerOptions)" />
    public override Lineage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var path = reader.GetString();
        return path == null ? null : new Lineage(path);
    }

    /// <inheritdoc cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)" />
    public override void Write(Utf8JsonWriter writer, Lineage value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Path, options);
    }

}
