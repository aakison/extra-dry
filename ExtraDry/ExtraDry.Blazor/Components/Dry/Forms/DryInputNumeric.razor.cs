using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a numeric input field. Prefer the use of <see cref="DryInput{T}" /> instead
/// of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputNumeric<T> : DryInputBase<T>
{

    /// <inheritdoc cref="DryInput{T}.ReadOnly" />
    [Parameter]
    public bool ReadOnly { get; set; }

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
            var validation = ValidateProperty();
            await InvokeOnChangeAsync(new ChangeEventArgs { Value = decimalValue });
            await InvokeOnValidationAsync(validation);
        }
        else {
            var validation = new ValidationEventArgs() { 
                IsValid = false, 
                MemberName = Property.Property.Name, 
                Message = "Invalid number format." 
            };
            await InvokeOnValidationAsync(validation);
        }
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

}
