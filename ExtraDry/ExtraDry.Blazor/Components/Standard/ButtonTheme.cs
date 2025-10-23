namespace ExtraDry.Blazor;

/// <summary>
/// The theme for a button which controls the visual rendering of the buttons within a theme, with
/// an intent for consistent styles across the site.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ButtonTheme
{
    /// <summary>
    /// A normal looking button that is used for most actions. Solid buttons with clear affordance.
    /// </summary>
    Normal,

    /// <summary>
    /// A button which hides away a bit but is still visually a button. Typically rendered with an
    /// outline and transparent background.
    /// </summary>
    Ghost,

    /// <summary>
    /// A button which doesn't present as a button but has a click action. Typically rendered as
    /// just a piece of text or an icon.  May have hover effects to indicate clickability.
    /// </summary>
    Unobtrusive,
}
