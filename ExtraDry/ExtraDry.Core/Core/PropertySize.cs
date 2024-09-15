namespace ExtraDry.Core;

/// <summary>
/// A semantic description of the size of a property, used to determine how to layout forms.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertySize {

    /// <summary>
    /// Not defined, allow the UI to determine an appropriate size
    /// </summary>
    Unset = 0,

    /// <summary>
    /// A small property, such as an int field, small enough to fit four fields on a single line.
    /// </summary>
    Small,

    /// <summary>
    /// A medium property, such as short text that can fit two to a line.
    /// </summary>
    Medium,

    /// <summary>
    /// A large property, usually would get it's own line but could share with a small field.
    /// </summary>
    Large,

    /// <summary>
    /// An extra large property, always gets its own line.
    /// </summary>
    Jumbo,
}
