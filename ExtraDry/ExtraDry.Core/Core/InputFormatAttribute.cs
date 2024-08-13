
namespace ExtraDry.Core;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class InputFormatAttribute : Attribute
{
    /// <summary>
    /// An override of the propertys type, to allow it to be displayed differently. Eg a DateTime property might only want a datepicker.
    /// </summary>
    public Type? DataTypeOverride { get; set; }

    /// <summary>
    /// For numeric inputs a unit is often needed to be displayed in the input to show the meaning of the value (eg. $, £, ℃, m)
    /// </summary>
    public string? UnitSymbol { get; set; }
}
