
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
    /// 
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Affordance { get; set; }
}
