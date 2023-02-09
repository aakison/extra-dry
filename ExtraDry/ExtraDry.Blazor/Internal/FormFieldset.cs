#nullable enable

namespace ExtraDry.Blazor.Internal;

/// <summary>
/// Represents an HTML fieldset, visually boxing groups of input controls.
/// </summary>
internal class FormFieldset {

    public FormFieldset(string legend, string name)
    {
        Legend = legend;
        Name = name;
        CssClass = Slug.ToSlug(Name);
    }

    public string Name { get; set; }

    /// <summary>
    /// The legend for the fieldset, typically from the `HeaderAttribute`.
    /// </summary>
    public string Legend { get; set; }

    /// <summary>
    /// The CSS class for the Fieldset and Legend.
    /// </summary>
    public string CssClass {  get; set; }

    /// <summary>
    /// The groups that are inside this fieldset.
    /// </summary>
    public List<FormGroup> Groups { get; } = new();

}
