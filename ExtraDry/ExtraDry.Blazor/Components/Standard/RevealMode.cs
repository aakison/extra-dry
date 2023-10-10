namespace ExtraDry.Blazor;

/// <summary>
/// The mode that a Reveal component can be used in.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RevealMode
{
    /// <summary>
    /// Use the built-in expand/collapse animation.  Also specify the Height attribute to adjust
    /// the fully expanded height.
    /// </summary>
    Expand,

    /// <summary>
    /// Use the built-in fade-in/fade-out animation.
    /// </summary>
    Fade,

    /// <summary>
    /// Do not provide any built-in animation, use CSS styles and the built-in CSS class changes
    /// to control the animation.
    /// </summary>
    CssOnly,

}
