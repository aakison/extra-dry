using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ExtraDry.Core;

/// <summary>
/// A JSON converter that will serialize a HierarchyId object so that it can be deserialised and used on the front end
/// </summary>
public class HierarchyIdConverter : JsonConverter<HierarchyId>
{
    public override HierarchyId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return HierarchyId.Parse(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, HierarchyId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
