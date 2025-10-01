using ExtraDry.Blazor;
using ExtraDry.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraDry.Blazor.Components.Standard;

public partial class InputFileField : ComponentBase, IInputField

{
    /// <summary>
    /// If the value is set, this becomes the display value and the file is assumed to be set.
    /// </summary>
    [Parameter]
    public string Value { get; set; } = "";

    [Parameter]
    public string Accept { get; set; } = "*.*";

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public string CssClass { get; set; } = "";

    [Parameter]
    public string Icon { get; set; } = "input-file";

    [Parameter]
    public string Affordance { get; set; } = "open-folder";

    [Parameter]
    public string Placeholder { get; set; } = "choose file...";

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
    #endregion

    private bool DisplayIcon => Icon != "";

    private bool DisplayAffordance => Affordance != "" && !ReadOnly;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "text", ReadOnlyCss, CssClass);

    private string DisplayValue => string.IsNullOrWhiteSpace(Value) ? Placeholder : Value;

    private string PlaceholderCssClass => string.IsNullOrWhiteSpace(Value) ? "placeholder" : "";

    private string DisplayValueCssClasses => DataConverter.JoinNonEmpty(" ", "value", PlaceholderCssClass, ReadOnlyCss);

    private string InputId { get; set; } = "";

    protected override void OnInitialized()
    {
        InputId = Id switch {
            "" => $"inputFileField{++instanceCount}",
            _ => Id,
        };
    }

    [Parameter]
    public EventCallback<InputFileChangeEventArgs> OnChange { get; set; }

    private async Task OnChangeAsync(InputFileChangeEventArgs e)
    {
        Value = e.File.Name;
        await OnChange.InvokeAsync(e);
    }

    private void OnValueChanged(string? value)
    {
        Value = value ?? "";
        //var value = args.Value;
        //var valid = ValidateProperty();
        ValueChanged.InvokeAsync(value);
        //await InvokeOnChangeAsync(value);
        //await InvokeOnValidationAsync(valid);
    }

    private static int instanceCount;

}
