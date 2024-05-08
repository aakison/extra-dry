namespace ExtraDry.Blazor.Models;

/// <summary>
/// The location of the validation message in relation to the input control.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ValidationRenderMode
{
    /// <summary>
    /// Render the content before the input control.
    /// </summary>
    Before,

    /// <summary>
    /// Render the content after the input control.
    /// </summary>
    After,

    /// <summary>
    /// Render the content instead of the input control.
    /// </summary>
    InsteadOf
}
