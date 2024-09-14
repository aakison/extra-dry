using System.Text.Json;

namespace ExtraDry.Core;

/// <summary>
/// Represents values stored as part of expansion data which can optionally be validated against 
/// an Expando Schema.
/// </summary>
[JsonConverter(typeof(ExpandoValuesConverter))]
public class ExpandoValues : Dictionary<string, object?>, IValidatableObject
{

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpandoValues"/> class with the default 
    /// equality comparer.
    /// </summary>
    public ExpandoValues() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpandoValues"/> class that uses the specified
    /// equality comparer.
    /// </summary>
    public ExpandoValues(IEqualityComparer<string> comparer) : base(comparer) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpandoValues"/> class with the default
    /// equality comparer that is initialized with the provided dictionary.
    /// </summary>
    public ExpandoValues(IDictionary<string, object?> dictionary) : base(dictionary) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpandoValues"/> class with the specified
    /// equality comparer that is initialized with the provided dictionary.
    /// </summary>
    public ExpandoValues(IDictionary<string, object?> dictionary, IEqualityComparer<string> comparer)
        : base(dictionary, comparer) { }

    /// <summary>
    /// The schema that defines the structure of the values.  If null, no validation is performed.
    /// </summary>
    [JsonIgnore]
    [NotMapped]
    public ExpandoSchema? Schema { get; set; }

    /// <summary>
    /// Validates the values against the schema if one is provided.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(Values.Any(e => e is JsonElement element && element.ValueKind == JsonValueKind.Array)) {
            yield return new ValidationResult("Arrays are not allowed as custom values.");
        }

        if(Values.Any(e => e is JsonElement element && element.ValueKind == JsonValueKind.Object)) {
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
