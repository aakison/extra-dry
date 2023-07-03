namespace ExtraDry.Blazor;

/// <summary>
/// Alters the size of the indicator icon
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum IndicatorSize {
    /// <summary>
    /// Displays as a small indicator icon
    /// </summary>
    Small,
    /// <summary>
    /// This is  the default indicator icon size
    /// </summary>
    Standard,
    /// <summary>
    /// Displays as a large indicator icon
    /// </summary>
    Large
}
