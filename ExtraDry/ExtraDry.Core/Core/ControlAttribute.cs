namespace ExtraDry.Core;

/// <summary>
/// Defines the display option for a property when it is displayed as a user interface control. Not
/// typically required, but can be used to change default control behavior, such as rendering a
/// radio button list instead of a select drop-down.
/// </summary>
/// <inheritdoc cref="ControlAttribute" />
[AttributeUsage(AttributeTargets.Property)]
public class ControlAttribute(ControlType type = ControlType.BestMatch) : Attribute
{
    /// <summary>
    /// The type of control to use when rendering the property.
    /// </summary>
    public ControlType Type { get; set; } = type;

    // TODO: Evaluate usefulness, seems to be used for incomplete Content control
    public string Icon { get; set; } = string.Empty;

    // TODO: Evaluate usefulness, seems to be used for incomplete Content control
    public string CaptionTemplate { get; set; } = "{0}";
}
