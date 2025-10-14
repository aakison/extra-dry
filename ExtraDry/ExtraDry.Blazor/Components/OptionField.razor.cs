namespace ExtraDry.Blazor.Components;

public partial class OptionField<T> : FieldBase<T> 
{
    /// <summary>
    /// Set of values to select from, any object can be used and the display text is either
    /// IResourceIdentifiers.Title or object.ToString() value.
    /// </summary>
    [Parameter, EditorRequired]
    public IList<T> Values { get; set; } = null!;

    #region For IValidatableField

    [Parameter]
    public object? ValidationModel { get; set; }

    [Parameter]
    public string ValidationProperty { get; set; } = "";

    #endregion

    private bool IsValid { get; set; } = true;

    public string ValidationMessage { get; set; } = "";

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "select", ReadOnlyCss, CssClass);

    private List<Option> Options { get; set; } = [];

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        
        Options = Values.Select(e => new Option(e)).ToList();
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

    public class Option
    {
        public Option(T source)
        {
            if(source is IResourceIdentifiers resource) {
                Key = resource.Uuid.ToString();
                DisplayText = resource.Title ?? source.ToString() ?? "--empty--";
            }
            else if(source is Enum enumValue) {
                Key = $"{source.GetType().Name}-{enumValue}";
                DisplayText = DataConverter.DisplayEnum(enumValue);
            }
            else {
                Key = Guid.NewGuid().ToString();
                DisplayText = source?.ToString() ?? "--empty--";
            }
            Value = source;
        }

        public string Key { get; init; }

        public string DisplayText { get; init; }

        public T Value { get; init; }
    }

    private async Task NotifyInputByUuid(ChangeEventArgs args)
    {
        var selected = Options.FirstOrDefault(e => e.Key == (string?)args.Value);
        var objectArgs = selected == null 
            ? new ChangeEventArgs { Value = null }
            : new ChangeEventArgs { Value = selected.Value };
        await NotifyInput(objectArgs);
    }

    private async Task NotifyChangeByUuid(ChangeEventArgs args)
    {
        var selected = Options.FirstOrDefault(e => e.Key == (string?)args.Value);
        var objectArgs = selected == null
            ? new ChangeEventArgs { Value = null }
            : new ChangeEventArgs { Value = selected.Value };
        await NotifyChange(objectArgs);
    }

}
