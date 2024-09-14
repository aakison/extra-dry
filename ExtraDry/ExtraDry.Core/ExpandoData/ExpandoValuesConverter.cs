using System.Text.Json;

namespace ExtraDry.Core;

/// <summary>
/// Provides a converter for ExpandoValues to allow for custom fields to be stored with entities.
/// Supports slightly different data type than JSON, dropping support for arrays and objects and
/// adding support for DateTime when the string format is exact.
/// </summary>
public class ExpandoValuesConverter : JsonConverter<ExpandoValues>
{

    /// <inheritdoc cref="JsonConverter{T}.Read(ref Utf8JsonReader, Type, JsonSerializerOptions)"/>
    public override ExpandoValues? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(ref reader, options);
        if(dictionary == null) {
            return default;
        }
        return new ExpandoValues(dictionary.ToDictionary(e => e.Key, e => UnpackJsonElement(e.Value)));
    }

    /// <inheritdoc cref="JsonConverter{T}.Write(Utf8JsonWriter, T, JsonSerializerOptions)" />
    public override void Write(Utf8JsonWriter writer, ExpandoValues value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (IDictionary<string, object>)value!, options);
    }

    /// <summary>
    /// When the serializer deserializes a dictionary from a json object, the values are JsonElement.
    /// During ExpandoValues deserialization, we want to convert these to the appropriate type.
    /// </summary>
    private static object? UnpackJsonElement(object? item)
    {
        if(item == null) {
            return null;
        }
        else if(item is JsonElement element) {
            return element.ValueKind switch {
                JsonValueKind.Array => throw new DryException("Custom dictionaries do not support arrays."),
                JsonValueKind.Object => throw new DryException("Custom dictionaries to not support objects."),
                JsonValueKind.String when element.TryGetDateTime(out DateTime dateValue) => dateValue,
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => element.GetDouble(),
                JsonValueKind.Null => null,
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                _ => throw new DryException("Unrecognized Json Element value kind, can't deserialize Expando Values.")
            };
        }
        else if(item is double || item is string || item is DateTime) {
            return item;
        }
        else {
            throw new ArgumentException("Elements in Expando Values must either be JsonElement, double, string, or DateTime.", nameof(item));
        }
    }

}
