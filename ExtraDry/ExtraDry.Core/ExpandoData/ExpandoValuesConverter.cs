using System.Text.Json;

namespace ExtraDry.Core;

public class ExpandoValuesConverter : JsonConverter<ExpandoValues> {

    public override ExpandoValues? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var fieldsDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(ref reader, options);
        if(fieldsDictionary == null) {
            return default;
        }

        return new ExpandoValues { Values = fieldsDictionary };
    }

    public override void Write(Utf8JsonWriter writer, ExpandoValues value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Values, options);
    }
}
