namespace ExtraDry.Core;

/// <summary>
/// Specifies a display-only formatter to use when rendering this property in a display context,
/// such as a details view or read-only field. The provided type must implement
/// <c>IValueFormatter</c> and have a public parameterless constructor. This formatter is used
/// for read-only display and does not affect input field editing.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class DisplayFormatterAttribute : Attribute
{

    public DisplayFormatterAttribute()
    {
    }

    public DisplayFormatterAttribute(Type? formatter = null)
    {
        Formatter = formatter;
    }

    /// <summary>
    /// The type of the formatter to use for  display. Must implement  <c>IValueFormatter</c>.
    /// </summary>
    public Type? Formatter { get; set; }

}
