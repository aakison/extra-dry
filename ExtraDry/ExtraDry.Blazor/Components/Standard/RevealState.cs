namespace ExtraDry.Blazor;

/// <summary>
/// Represents the state of a Reveal component as it cycles from not being loaded, through
/// revealing and back.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RevealState
{
    /// <summary>
    /// The Reveal component is completely concealed and is not in the DOM.
    /// </summary>
    None,

    /// <summary>
    /// The Reveal component is collapsed/hidden, but the DOM has been rendered and is just not
    /// visible. All contents are clipped.
    /// </summary>
    Concealed,

    /// <summary>
    /// The Reveal component is being expanded/shown and is in the period of animation revealing
    /// the content. All contents are clipped.
    /// </summary>
    Revealing,

    /// <summary>
    /// The Reveal component has completed expanding/showing and ix currently Revealed. Contents
    /// are no longer clipped, allowing forms to be shown outside of bounds.
    /// </summary>
    Revealed,

    /// <summary>
    /// The Reveal component is being collapsed/hidden and is in the period of animation concealing
    /// the content. All contents are clipped.
    /// </summary>
    Concealing,
}
