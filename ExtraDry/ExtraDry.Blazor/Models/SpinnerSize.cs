namespace ExtraDry.Blazor;

/// <summary>
/// Alters the size of the spinner
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SpinnerSize {
    /// <summary>
    /// Displays as a small spinner
    /// </summary>
    Small,
    /// <summary>
    /// This is  the default spinner size
    /// </summary>
    Standard,
    /// <summary>
    /// Displays as a large spinner
    /// </summary>
    Large
}
