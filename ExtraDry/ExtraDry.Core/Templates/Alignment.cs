namespace ExtraDry.Core;

/// <summary>
/// The types of alignment that can be used by the
/// </summary>
public enum Alignment {

    /// <summary>
    /// Indicates that the form alignment is not known or not specified.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The form alignment should be flush left.
    /// </summary>
    Left,

    /// <summary>
    /// The form alignment should be flush right.
    /// </summary>
    Right,

    /// <summary>
    /// The form alignment should be centered.
    /// </summary>
    Center,

    /// <summary>
    /// The form alignment should be justified between the margins.
    /// </summary>
    Justify
}
