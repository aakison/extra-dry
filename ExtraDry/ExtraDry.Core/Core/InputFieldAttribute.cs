namespace ExtraDry.Core;

/// <summary>
/// Defines a set of properties to inform the UI how to render this property. Provides further
/// information such as type overrides and icons.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class InputFieldAttribute : Attribute
{
    /// <summary>
    /// Provides a means to override the default input that is rendered for this property.
    /// </summary>
    public Type? DataType { get; set; }

    /// <summary>
    /// The icon to be used in the display of this property's input, typically on the left of the
    /// input
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// The affordance icon to be used in the display of this property's input, typically on the
    /// right of the input.
    /// </summary>
    public string? Affordance { get; set; }

    // <summary>
    // Gets or sets the size that this field should be displayed.  If this property is not set
    // then the presentation layer will automatically determine the size.  Setting this property
    // explicitly allows an override of the default behavior of the presentation layer.
    // </summary>
    //public PropertySize SizeOverride { get; set; }
}
