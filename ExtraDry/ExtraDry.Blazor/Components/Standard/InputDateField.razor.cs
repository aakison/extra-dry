using ExtraDry.Blazor;
using ExtraDry.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ExtraDry.Blazor.Components.Standard;

public partial class InputDateField : ComponentBase, IInputField<string>
{

    [Parameter]
    public string Value { get; set; } = "";

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

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

    #region For IInputField<string>

    /// <inheritdoc />
    [Parameter]
    public PropertySize Size { get; set; } = PropertySize.Medium;

    /// <inheritdoc />
    [Parameter]
    public string Description { get; set; } = "";

    /// <inheritdoc />
    [Parameter]
    public bool ShowDescription { get; set; } = false;

    /// <inheritdoc />
    [Parameter]
    public string Caption { get; set; } = "";

    /// <inheritdoc />
    [Parameter]
    public bool ShowLabel { get; set; } = true;

    #endregion

    #region For IValidatableField ??? To create if good idea...

    [Parameter]
    public object? ValidationModel { get; set; }

    [Parameter]
    public string ValidationProperty { get; set; } = "";

    #endregion

    private bool IsValid { get; set; } = true;

    public string ValidationMessage { get; set; } = "";

    private bool DisplayIcon => Icon != "";

    private bool DisplayAffordance => Affordance != "" && !ReadOnly;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "date", ReadOnlyCss, IsValidCss, CssClass);

    private string InputId { get; set; } = "";

    private string IsValidCss => IsValid ? "valid" : "invalid";

    protected override void OnInitialized()
    {
        InputId = Id switch {
            "" => $"inputDateField{++instanceCount}",
            _ => Id,
        };
    }

    private async Task OnDateChanged(ChangeEventArgs e)
    {
        var value = e.Value?.ToString() ?? "";
        Value = value;
        await ValueChanged.InvokeAsync(value);
        Validate();
    }

    private void Validate()
    {
        if(ValidationModel == null) {
            return;
        }
        var validator = new DataValidator();
        if(validator.ValidateProperties(ValidationModel, ValidationProperty)) {
            UpdateValidationUI(true, string.Empty);
        }
        else {
            UpdateValidationUI(false, string.Join("; ", validator.Errors.Select(e => e.ErrorMessage)));
        }
    }

    private void UpdateValidationUI(bool valid, string message)
    {
        IsValid = valid;
        ValidationMessage = DryValidationSummary.FormatMessage(ValidationProperty, message);
        StateHasChanged();
    }

    private static int instanceCount;

}
