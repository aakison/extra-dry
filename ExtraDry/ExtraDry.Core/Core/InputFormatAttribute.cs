
namespace ExtraDry.Core;

/// <summary>
/// Defines a set of properties to inform the UI how to render this property. Provides further information such as type overrides and icons.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class InputFormatAttribute : Attribute
{
    /// <summary>
    /// Provides a means to override the default input that is rendered for this property.
    /// </summary>
    public Type? DataTypeOverride { get; set; }

    /// <summary>
    /// The icon to be used in the display of this property's input, typically on the left of the input
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// The affordance icon to be used in the display of this property's input, typically on the right of the input.
    /// </summary>
    public string? Affordance { get; set; }

    public PropertySize Size { get; set; } = PropertySize.Calculated;
}
