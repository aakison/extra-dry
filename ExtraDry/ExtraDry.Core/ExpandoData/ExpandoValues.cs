using System.Text.Json;

namespace ExtraDry.Core.Templates {

    [JsonConverter(typeof(ExpandoValuesConverter))]
    public class ExpandoValues : IValidatableObject {
        
        public Dictionary<string, object> Values { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Values.Values.Any(e => e is JsonElement && ((JsonElement)e).ValueKind == JsonValueKind.Array)) {
                yield return new ValidationResult("Arrays are now allowed as custom values.");
            }

            if(Values.Values.Any(e => e is JsonElement && ((JsonElement)e).ValueKind == JsonValueKind.Object)) {
                yield return new ValidationResult("Complex Objects are now allowed as custom values.");
            }
        }
    }
}
