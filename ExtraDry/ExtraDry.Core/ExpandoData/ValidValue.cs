namespace ExtraDry.Core;

/// <summary>
/// Represents a single discrete value that is acceptable for user input.
/// </summary>
public class ValidValue {
    /// <summary>
    /// The actual value that is stored in the database represents this valid value.
    /// </summary>

    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// The relative order of this value when being presented alongside other values (in descending order).
    /// There is no requirement that this is an absolute number, just relative to other values.  E.g. orders of (-17, 1, 2, 3, 109, 109) are acceptable.
    /// If there is a tie in the order, then the display should be done lexicographically by the Value property
    /// </summary>

    public int Order { get; set; }

    /// <summary>
    /// The name of an Image that is associated with this value.
    /// Mobile apps will have a set of known icons that can be used and are matched by name.
    /// If the name is not know in the app, then the app will display the Field.DefaultIcon.
    /// </summary>

    public string Icon { get; set; } = string.Empty;

}
