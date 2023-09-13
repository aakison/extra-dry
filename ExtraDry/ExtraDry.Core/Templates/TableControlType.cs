namespace ExtraDry.Core;

/// <summary>
/// The type for the table control that is used to display the element in a table.
/// </summary>
public enum TableControlType {

    /// <summary>
    /// The control type is not known, not typically valid but may occur when deserialized.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// A control that simply displays the content, the most typical control type.
    /// </summary>
    Display,

    /// <summary>
    /// Indicates that the field references a pick-list that is associated with an icon that can be displayed instead of the text.
    /// </summary>
    Icon,

    /// <summary>
    /// A meta-data control that does not need to be tied to a Field.
    /// This is used, typically by Zuuse Capture, to show when a tag has been modified but not synhronized.
    /// </summary>
    ModifiedIcon,

    /// <summary>
    /// A meta-data control that does not need to be tied to a Field.
    /// This is used, typically by Zuuse Capture, to show when a tag has met the completed criteria.
    /// </summary>
    CompletedIcon,

    /// <summary>
    /// A meta-data control that does not need to be tied to a Field.
    /// This is used, typically by Zuuse Capture, to show an icon that indicates if there is an associated document, typically a photo.
    /// </summary>
    AttachmentIcon,
}
