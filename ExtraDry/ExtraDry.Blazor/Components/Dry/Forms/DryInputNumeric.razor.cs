using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a numeric input field. Prefer the use of <see cref="DryInput{T}" /> instead
/// of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputNumeric<T> : ComponentBase, IDryInput<T>, IExtraDryComponent
{
    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc />
    [Parameter, EditorRequired]
    public T? Model { get; set; }

    /// <inheritdoc />
    [Parameter, EditorRequired]
    public PropertyDescription? Property { get; set; }

    /// <inheritdoc />
    [Parameter]
    public EventCallback<ChangeEventArgs>? OnChange { get; set; }

    /// <inheritdoc cref="Blazor.EditMode" />
    [CascadingParameter]
    public EditMode EditMode { get; set; } = EditMode.Create;

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    protected override void OnParametersSet()
    {
        if(Model == null || Property == null) {
            return;
        }
        Value = Property.DisplayValue(Model);
    }

    [Inject]
    private ILogger<DryInput<T>> Logger { get; set; } = null!;

    private string Icon => Property?.InputFormat?.Icon ?? "";

    private string Affordance => Property?.InputFormat?.Affordance ?? "";

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", ReadOnlyCss, CssClass);

    private string InputTitle => Property?.FieldCaption ?? "";

    [Parameter]
    public string? Placeholder { get; set; }

    private string PlaceholderDisplay => Placeholder ?? Property?.Display?.Prompt ?? "";

    private async Task HandleChange(ChangeEventArgs args)
    {
        if(Property == null || Model == null) {
            return;
        }

        var value = args.Value?.ToString()?.Replace(",", "") ?? "";

        var valid = false;
        if(decimal.TryParse(value, CultureInfo.CurrentCulture, out var decimalValue)) {
            Value = DisplayValue(decimalValue);
            SetProperty(decimalValue);
            valid = true;
        }
        else {
            valid = false;
        }
        await OnValidation.InvokeAsync(new ValidationEventArgs {
            IsValid = valid,
            MemberName = Property.PropertyType.Name,
            Message = valid ? string.Empty : $"Not a valid number.",
        });

    }

    private string DisplayValue(decimal decimalValue)
    {
        if(Property == null || Model == null) {
            return "";
        }
        return Property.InputType switch {
            Type t when t == typeof(int) => decimalValue.ToString("#,#", CultureInfo.CurrentCulture),
            Type t when t == typeof(decimal) => decimalValue.ToString("#,0.00", CultureInfo.CurrentCulture),
            _ => throw new NotImplementedException("Could not map type to property."),
        };
    }

    private void SetProperty(decimal decimalValue)
    {
        if(Property == null || Model == null) {
            return;
        }
        var value = Property.PropertyType switch {
            Type t when t == typeof(int) => (int)decimalValue,
            Type t when t == typeof(decimal) => decimalValue,
            _ => throw new NotImplementedException("Could not map type to property."),
        };
        Property.SetValue(Model, value);
    }

    /// <summary>
    /// Event that is raised when the input is validated using internal rules. Does not check
    /// global rules that might be set on the model using data annotations.
    /// </summary>
    [Parameter]
    public EventCallback<ValidationEventArgs> OnValidation { get; set; }


    private async Task CallOnChange()
    {
        var task = OnChange?.InvokeAsync();
        if(task != null) {
            await task;
        }
    }

    private string Value = "";

    /// <summary>
    /// Only allow digits, commas, periods and navigation keys. 
    /// </summary>
    private static string DisableInvalidCharacters => @"if(!(/[0-9.,]/.test(event.key) || event.key == 'Backspace' || event.key == 'Delete' || event.key == 'ArrowLeft' || event.key == 'ArrowRight' || event.key == 'Tab')) return false;";

}
