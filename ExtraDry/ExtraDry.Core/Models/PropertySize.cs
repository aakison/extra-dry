namespace ExtraDry.Core;

/// <summary>
/// A semantic description of the size of a property, used to determine how to layout forms.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PropertySize
{
    /// <summary>
    /// The size is determined automatically based on context.
    /// </summary>
    Auto = 0,

    /// <summary>
    /// A small property, such as an int field, small enough to fit four fields on a single line.
    /// </summary>
    Small = 1,

    /// <summary>
    /// A medium property, such as short text that can fit two to a line.
    /// </summary>
    Medium = 2,

    /// <summary>
    /// A large property, usually would get it's own line but could share with a small field.
    /// </summary>
    Large = 3,

    /// <summary>
    /// An extra large property, always gets its own line.
    /// </summary>
    Jumbo = 4,
}
