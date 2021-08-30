#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sample.Shared.Converters {
    public class SectorInfoJsonConverter : JsonConverter<Sector> {
        public override Sector Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if(reader.TokenType != JsonTokenType.StartObject) {
                throw new JsonException();
            }
            var sector = new Sector();
            while(reader.Read()) {
               if(reader.TokenType == JsonTokenType.PropertyName) {
                    var property = reader.GetString();
                    reader.Read();
                    if(reader.TokenType == JsonTokenType.String) {
                        var value = reader.GetString();
                        switch(property) {
                            case nameof(Sector.UniqueId):
                                sector.UniqueId = Guid.Parse(value);
                                break;
                            case nameof(Sector.Title):
                                sector.Title = value;
                                break;
                        }
                    }
                    else {
                        throw new JsonException();
                    }
                }
                else if(reader.TokenType == JsonTokenType.EndObject) {
                    return sector;
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Sector sector, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString(nameof(sector.UniqueId), sector.UniqueId);
            writer.WriteString(nameof(sector.Title), sector.Title);
            writer.WriteEndObject();
        }
    }
}
