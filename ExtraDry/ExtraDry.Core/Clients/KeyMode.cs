namespace ExtraDry.Core;

/// <summary>
/// Determines the method by which the key is applied to the endpoint URL for CRUD operations. 
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum KeyMode
{
    /// <summary>
    /// Appends the stringified key to the endpoint URL.
    /// </summary>
    Append,

    /// <summary>
    /// Use the EndpointFormatters to replace parameter placeholders in the endpoint URL with formatted values derived from the key. 
    /// </summary>
    Formatters,

    /// <summary>
    /// No endpoint transforms are applied.
    /// </summary>
    None,
}
