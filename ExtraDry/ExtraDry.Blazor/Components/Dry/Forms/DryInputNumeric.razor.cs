using System.Reflection.Metadata;

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

    /// <inheritdoc cref="DryInput{T}.ReadOnly" />
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Event that is raised when the input is validated using internal rules. Does not check
    /// global rules that might be set on the model using data annotations.
    /// </summary>
    [Parameter]
    public EventCallback<ValidationEventArgs> OnValidation { get; set; }

    protected override void OnParametersSet()
    {
        if(Model == null || Property == null) {
            return;
        }
        var objValue = Property.GetValue(Model) ?? 0.0m;
        Value = DisplayValue((decimal)objValue);
    }

    /// <summary>
    /// Only allow digits, commas, periods and navigation keys. 
    /// </summary>
    private static string DisableInvalidCharacters => @"if(!(/[0-9.,]/.test(event.key) || event.key == 'Backspace' || event.key == 'Delete' || event.key == 'ArrowLeft' || event.key == 'ArrowRight' || event.key == 'Tab')) return false;";

    [Inject]
    private ILogger<DryInput<T>> Logger { get; set; } = null!;

    private string Icon => Property?.InputFormat?.Icon ?? "";

    private string Affordance => Property?.InputFormat?.Affordance ?? "";

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", ReadOnlyCss, CssClass);

    private string InputTitle => Property?.FieldCaption ?? "";

    private string Value { get; set; } = "";

    private async Task HandleChange(ChangeEventArgs args)
    {
        if(Property == null || Model == null) {
            return;
        }

        var value = args.Value?.ToString()?.Replace(",", "") ?? "";

        var valid = false;
        if(decimal.TryParse(value, CultureInfo.CurrentCulture, out var decimalValue)) {
            SetProperty(decimalValue);
            var newValue = DisplayValue(decimalValue);
            if(Value == newValue) {
                // rare case where Value property doesn't change because of formatting issues.  E.g. 123.45 written as 1,2,3.45 won't trigger Value change refresh.
                Value = $" {newValue}"; // stringly different but minimize UI flicker
                StateHasChanged();
                await Task.Delay(1); // let the UI update
            }
            Value = newValue;
            StateHasChanged();
            valid = AssertValid();
            await InvokeOnChange(new ChangeEventArgs { Value = decimalValue });
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

    private bool AssertValid()
    {
        bool valid = true;
        var validator = new DataValidator();
        validator.ValidateProperties(Model!, Property!.Property.Name);
        valid = validator.Errors.Count == 0;
        return valid;
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

    private async Task InvokeOnChange(ChangeEventArgs args)
    {
        var task = OnChange?.InvokeAsync(args);
        if(task != null) {
            await task;
        }
    }
}
