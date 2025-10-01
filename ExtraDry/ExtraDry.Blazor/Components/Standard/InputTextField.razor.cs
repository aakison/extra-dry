namespace ExtraDry.Blazor.Components.Standard;

public partial class InputTextField : ComponentBase, IInputField<string>
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

    [Parameter]
    public int MaxLength { get; set; } = 1_000_000;

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

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "text", ReadOnlyCss, IsValidCss, CssClass);

    private string InputId { get; set; } = "";

    private string IsValidCss => IsValid ? "valid" : "invalid";

    protected override void OnInitialized()
    {
        InputId = Id switch {
            "" => $"inputTextField{++instanceCount}",
            _ => Id,
        };
    }

    private void OnValueChanged(string? value)
    {
        Value = value ?? "";
        ValueChanged.InvokeAsync(value);
        Validate();
        //await InvokeOnChangeAsync(value);
        //await InvokeOnValidationAsync(valid);
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
        // Remove common redundant portions of messages
        Console.WriteLine($"Validating UI... {valid} {message}");
        IsValid = valid;
        ValidationMessage = DryValidationSummary.FormatMessage(ValidationProperty, message);
        StateHasChanged();
    }

    private static int instanceCount;

}
