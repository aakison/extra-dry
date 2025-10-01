using ExtraDry.Blazor;
using ExtraDry.Core;
using Microsoft.AspNetCore.Components;

namespace ExtraDry.Blazor.Components.Standard;

public partial class InputSelectField : ComponentBase
{
    [Parameter, EditorRequired]
    public IDictionary<string, string> Data { get; set; } = null!;

    [Parameter]
    public string? Value { get; set; }

    [Parameter]
    public EventCallback<string?> ValueChanged { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public string CssClass { get; set; } = "";

    [Parameter]
    public string Icon { get; set; } = "";

    [Parameter]
    public string Affordance { get; set; } = "";

    [Parameter]
    public string Placeholder { get; set; } = "";

    [Parameter]
    public string Id { get; set; } = "";

    #region For Frame

    [Parameter]
    public PropertySize Size { get; set; } = PropertySize.Medium;

    [Parameter]
    public string Description { get; set; } = "";

    [Parameter]
    public bool ShowDescription { get; set; } = false;

    [Parameter]
    public string Caption { get; set; } = "";

    [Parameter]
    public bool ShowLabel { get; set; } = true;

    #endregion For Frame

    protected override void OnInitialized()
    {
        InputId = Id switch {
            "" => $"inputSelectField{++instanceCount}",
            _ => Id,
        };
    }

    private bool DisplayIcon => Icon != "";

    private bool DisplayOptions => !ReadOnly;

    private bool DisplayAffordance => Affordance != "" && !ReadOnly;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "select", ReadOnlyCss, CssClass);

    private string InputId { get; set; } = "";

    private void OnValueChanged(string? value)
    {
        ValueChanged.InvokeAsync(value);
    }

    private static int instanceCount;
}
