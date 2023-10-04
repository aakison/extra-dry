using System.Text.Json;

namespace ExtraDry.Core; 

/// <summary>
/// Represents values for User Defined Fields.
/// </summary>
public class ExpandoValues : IValidatableObject {

    public Dictionary<string, object> Values { get; set; } = new();

    [JsonIgnore]
    [NotMapped]
    public ExpandoSchema? Schema { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(Values.Values.Any(e => e is JsonElement && ((JsonElement)e).ValueKind == JsonValueKind.Array)) {
            yield return new ValidationResult("Arrays are not allowed as custom values.");
        }

        if(Values.Values.Any(e => e is JsonElement && ((JsonElement)e).ValueKind == JsonValueKind.Object)) {
            yield return new ValidationResult("Complex Objects are not allowed as custom values.");
        }

        if(Schema != null) {
            var results = Schema.ValidateValues(this);
            foreach(var result in results) {
                yield return result;
            }
        }
    }
}
