namespace ExtraDry.Blazor;

/// <summary>
/// The theme for a button which controls the visual rendering of the buttons within a theme, with 
/// an intent for consistent styles across the site.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ButtonTheme
{
    /// <summary>
    /// A button that stands out as the obvious button to click to complete your page.  Typically
    /// rendered with the primary color of the theme.
    /// </summary>
    Action,

    /// <summary>
    /// A normal looking button that is used for most actions.  Typically rendered with a nuetral
    /// color theme.
    /// </summary>
    Normal,

    /// <summary>
    /// A button which hides away a bit but is still clearly a button.  Typically rendered with an
    /// outline and transparent background.
    /// </summary>
    Ghost,

    /// <summary>
    /// A button which doesn't present as a button but has a click action.  Typically rendered as
    /// just a piece of text or an icon.
    /// </summary>
    Unobtrusive,
}
