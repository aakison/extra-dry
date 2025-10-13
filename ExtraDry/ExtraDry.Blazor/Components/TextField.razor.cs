namespace ExtraDry.Blazor.Components;

public partial class TextField : FieldBase<string>
{

    [Parameter]
    public int MaxLength { get; set; } = 1_000_000;


    #region For IValidatableField ??? To create if good idea...

    [Parameter]
    public object? ValidationModel { get; set; }

    [Parameter]
    public string ValidationProperty { get; set; } = "";

    #endregion

    private bool IsValid { get; set; } = true;

    public string ValidationMessage { get; set; } = "";


    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "text", ReadOnlyCss, IsValidCss, CssClass);

    private string IsValidCss => IsValid ? "valid" : "invalid";


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

}
