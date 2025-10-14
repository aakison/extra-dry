namespace ExtraDry.Blazor.Components;

public partial class TextField : FieldBase<string>
{

    [Parameter]
    public int MaxLength { get; set; } = 1_000_000;

    [Parameter]
    public override PropertySize Size { get; set; } = PropertySize.Large;

    #region For IValidatableField ??? To create if good idea...

    [Parameter]
    public object? ValidationModel { get; set; }

    [Parameter]
    public string ValidationProperty { get; set; } = "";

    #endregion

    private bool IsValid { get; set; } = true;

    public string ValidationMessage { get; set; } = "";

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", ModeCss, ReadOnlyCss, IsValidCss, CssClass);

    private string ModeCss => IsMultiline ? "textarea" : "text";

    private string IsValidCss => IsValid ? "valid" : "invalid";

    private bool IsMultiline => MaxLength > StringLength.Line;

    private string TextSize {
        get {
            if(MaxLength <= StringLength.Word) {
                return nameof(StringLength.Word).ToLowerInvariant();
            }
            else if(MaxLength <= StringLength.Words) {
                return nameof(StringLength.Words).ToLowerInvariant();
            }
            else if(MaxLength <= StringLength.Line) {
                return nameof(StringLength.Line).ToLowerInvariant();
            }
            else if(MaxLength <= StringLength.Sentence) {
                return nameof(StringLength.Sentence).ToLowerInvariant();
            } 
            else if(MaxLength <= StringLength.Paragraph) {
                return nameof(StringLength.Paragraph).ToLowerInvariant();
            }
            else if(MaxLength <= StringLength.Page) {
                return nameof(StringLength.Page).ToLowerInvariant();
            }
            else {
                return nameof(StringLength.Book).ToLowerInvariant();
            }
        }
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

}
