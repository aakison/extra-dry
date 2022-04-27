#nullable enable

namespace ExtraDry.Blazor.Internal;

/// <summary>
/// Represents an HTML fieldset, visually boxing groups of input controls.
/// </summary>
internal class FormFieldset {

    public string Legend { get; set; } = string.Empty;

    public List<FormGroup> Groups { get; } = new();

}
