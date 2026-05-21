namespace ExtraDry.Core;

/// <summary>
/// Specifies a display-only formatter to use when rendering this property in a table column.
/// The provided type must implement <c>IValueFormatter</c> and have a public parameterless
/// constructor. This formatter is used for read-only column display and does not affect input
/// field editing.
/// </summary>
/// <remarks>
/// Use this attribute when the column display format differs from the editable input format,
/// e.g. showing a relative time ("2 hours ago") in a table while still editing as a date/time
/// in a form.
/// </remarks>
[AttributeUsage(AttributeTargets.Property)]
public class TableColumnAttribute : Attribute
{

    public TableColumnAttribute()
    {
    }

    public TableColumnAttribute(string caption, Type? formatterType = null)
    {
        Caption = caption;
        FormatterType = formatterType;
    }
    
    /// <summary>
    /// The type of the formatter to use for column display. Must implement
    /// <c>IValueFormatter</c>.
    /// </summary>
    public Type? FormatterType { get; set; }

    /// <summary>
    /// The order in the DryTable to display the column.
    /// </summary>
    public int Order {
        get {
            if(!order.HasValue) {
                throw new InvalidOperationException("Order property has not been set, use GetOrder() to get value or null.");
            }
            return order.GetValueOrDefault();
        }
        set => order = value;
    }
    private int? order = 0;

    public int? GetOrder() => order;

    public string? Caption { get; set; }

    /// <summary>
    /// The width of the column, e.g. "10fr" or "8em". Uses CSS grid syntax for column sizing.
    /// If not specified, the column will auto-size based on property settings.  Use "em" sizing
    /// when content is relatively stable width, e.g. phone numbers.
    /// </summary>
    public string? Width { get; set; }
}
