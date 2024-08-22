
namespace ExtraDry.Core;

/// <summary>
/// Defines a set of properties to inform the UI how to render this property. Provides further information such as type overrides and icons.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class InputFormatAttribute : Attribute
{
    private PropertySize? size;

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

    /// <summary>
    /// Gets or sets the size that this field should be displayed.  If this property is not set 
    /// then the presentation layer will automatically determine the size.  Setting this property
    /// explicitly allows an override of the default behavior of the presentation layer.
    /// </summary>
    /// <remarks>
    /// Consumers must use the <see cref="GetSize" /> method to retrieve the value, as this
    /// property getter will throw an exception if the value has not been set.
    /// </remarks>
    /// <exception cref="System.InvalidOperationException">
    /// If the getter of this property is invoked when the value has not been explicitly set using
    /// the setter.
    /// </exception>
    public PropertySize Size {
        get {
            if(!size.HasValue) {
                throw new InvalidOperationException($"The {nameof(Size)} property has not been set.  Use the {nameof(GetSize)} method to get the value.");
            }

            return size.GetValueOrDefault();
        }
        set => size = value;
    }

    public PropertySize? GetSize() => size;
}
