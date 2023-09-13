namespace ExtraDry.Core;

/// <summary>
/// An enumeration that controls the layout of a FormGroup.
/// </summary>
public enum FormOrientation {

    /// <summary>
    /// The layout is not provided or is not known.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Request to lay out elements of the group vertically on top of each other.
    /// </summary>
    Vertical,

    /// <summary>
    /// Request to lay out elements of the group horizontally beside each other.
    /// </summary>
    Horizontal,

    /// <summary>
    /// Request to lay out the elements of the group as a set of tabs.
    /// This only makes sense when the elements of the group are other groups.
    /// </summary>
    Tabbed
}
