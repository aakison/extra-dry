using ExtraDry.Core;

namespace ExtraDry.Blazor.Components;

/// <summary>
/// A component for editing an <see cref="ExpandoValues"/> collection based on a provided
/// <see cref="ExpandoSchema"/>. Iterates over the schema fields and renders the appropriate
/// input for each, ensuring dictionary entries exist for all fields.
/// </summary>
public partial class DryExpandoValuesField : ComponentBase
{
    /// <summary>
    /// The values dictionary to read from and write to.
    /// </summary>
    [Parameter, EditorRequired]
    public ExpandoValues Values { get; set; } = null!;

    /// <summary>
    /// The schema that describes the fields to render. When null or empty, nothing is rendered.
    /// </summary>
    [Parameter]
    public ExpandoSchema? Schema { get; set; }

    /// <summary>
    /// Raised after any field value changes.
    /// </summary>
    [Parameter]
    public EventCallback<ChangeEventArgs> OnChange { get; set; }

    /// <inheritdoc cref="Blazor.EditMode" />
    [CascadingParameter]
    public EditMode EditMode { get; set; } = EditMode.Create;

    protected override void OnParametersSet()
    {
        if(Schema == null || Values == null) {
            return;
        }

        // Ensure every field has an entry in the dictionary (with null / default value).
        foreach(var field in Schema.Fields) {
            if(!Values.ContainsKey(field.Slug)) {
                Values[field.Slug] = field.DefaultValue;
            }
        }
    }

    private bool IsReadOnly => EditMode == EditMode.ReadOnly;

    private string GetStringValue(string slug) => Values.TryGetValue(slug, out var v) ? v?.ToString() ?? "" : "";

    private double? GetNumberValue(string slug)
    {
        if(!Values.TryGetValue(slug, out var v) || v == null) {
            return null;
        }
        return v switch {
            double d => d,
            int i => (double)i,
            _ => double.TryParse(v.ToString(), out var parsed) ? parsed : null,
        };
    }

    private bool? GetBoolValue(string slug)
    {
        if(!Values.TryGetValue(slug, out var v) || v == null) {
            return null;
        }
        return v switch {
            bool b => b,
            _ => bool.TryParse(v.ToString(), out var parsed) ? parsed : null,
        };
    }

    private async Task HandleStringChange(string slug, ChangeEventArgs args)
    {
        Values[slug] = args.Value?.ToString();
        await OnChange.InvokeAsync(args);
    }

    private async Task HandleNumberChange(string slug, ChangeEventArgs args)
    {
        if(args.Value is double d) {
            Values[slug] = d;
        }
        else if(double.TryParse(args.Value?.ToString(), out var parsed)) {
            Values[slug] = parsed;
        }
        else {
            Values[slug] = null;
        }
        await OnChange.InvokeAsync(args);
    }

    private async Task HandleBoolChange(string slug, ChangeEventArgs args)
    {
        if(args.Value is bool b) {
            Values[slug] = b;
        }
        else if(bool.TryParse(args.Value?.ToString(), out var parsed)) {
            Values[slug] = parsed;
        }
        else {
            Values[slug] = null;
        }
        await OnChange.InvokeAsync(args);
    }
}
