namespace ExtraDry.Core;

/// <summary>
/// An enumeratio nthat controls the highlight that is applied to a FormGroup.
/// </summary>
public enum FormHighlight {

    /// <summary>
    /// The highlight is not provided or is not known.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// An explicit request to provide no highlighting and to have the groups elements blend in with the background.
    /// </summary>
    None,

    /// <summary>
    /// A request to have darker background for the group to set it apart from other elements.
    /// </summary>
    Dark,

    /// <summary>
    /// A request to have a lighter backgroudn for the group to set it apart from other elements.
    /// </summary>
    Light,

    /// <summary>
    /// A request to highlight the contents of the group in such a way as to convey a warning, such as bold red text for all contents.
    /// </summary>
    Warning
}
