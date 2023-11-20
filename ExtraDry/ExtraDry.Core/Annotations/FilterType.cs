namespace ExtraDry.Core;

/// <summary>
/// The processing rule to be applied to a property when a <see cref="FilterQuery"/>, 
/// <see cref="SortQuery"/> or <see cref="PageQuery"/> has a 
/// <see cref="FilterAttribute"/> provided.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FilterType
{

    /// <summary>
    /// Performs an exact match of the value, the default behavior.
    /// </summary>
    Equals = 0,

    /// <summary>
    /// If the property is a `string`, then the filter matches when the text matches the start of 
    /// the property.
    /// </summary>
    StartsWith,

    /// <summary>
    /// If the property is a `string`, then the filter matches when the text occurs anywhere 
    /// within the property.
    /// </summary>
    Contains,

}
