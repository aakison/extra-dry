using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExtraDry.Server;

public static class PropertyBuilderExtensions
{
    /// <summary>
    /// Sets a property to have a JSON serializer/deserializer roundtrip to be stored in a single
    /// column. Also adds metadata so that EF can detect when any portion of the entity has
    /// changed.
    /// </summary>
    /// <remarks>
    /// While .Equals and/or IEquatable overrides would be more efficient, the correctness of this
    /// solution stays aligned with the Json Payload which is the definition of whether it has
    /// actually changed.
    /// </remarks>
    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> source) where T : class
    {
        source.IsRequired()
            .HasConversion(
                e => Serialize(e),
                e => Deserialize<T>(e))
            .Metadata.SetValueComparer(new ValueComparer<T>(
                    (lhs, rhs) => Serialize(lhs) == Serialize(rhs),
                    e => Serialize(e).GetHashCode(),
                    e => Deserialize<T>(Serialize(e))
                )
            );
        // Prefer empty JSON in metadata and not .HasDefaultValue(new T()) above. Default json
        // documents can be quite large.
        source.Metadata.SetDefaultValueSql("'{}'");
        return source;
    }

    private static string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, options);
    }

    private static T Deserialize<T>(string s)
    {
        try {
            var val = JsonSerializer.Deserialize<T>(s, options)
                ?? throw new ArgumentException("Unable to deserialize", nameof(s));
            return val;
        }
        catch(JsonException ex) {
            throw new ArgumentException($"Couldn't deserialize '{s}'; {ex.Message}");
        }
    }

    private static readonly JsonSerializerOptions options = new();
}
