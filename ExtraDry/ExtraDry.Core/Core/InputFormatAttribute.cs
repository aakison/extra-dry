
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
    /// For numeric inputs a unit is often needed to be displayed in the input to show the meaning of the value (eg. $, £, ℃, m)
    /// </summary>
    public string? UnitSymbol { get; set; }
}
