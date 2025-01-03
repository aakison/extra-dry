using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a numeric input field. Prefer the use of <see cref="DryInput{T}" />
/// instead of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputNumeric<T>
    : DryInputBase<T>
    where T : class
{
    /// <inheritdoc cref="DryInput{T}.ReadOnly" />
    [Parameter]
    public bool ReadOnly { get; set; }

    protected override void OnParametersSet()
    {
        var objValue = Property.GetValue(Model);
        Value = Property.Formatter.Format(objValue);
    }

    /// <summary>
    /// Only allow digits, commas, periods and dollar signs. Injected into onkeypress event (not
    /// onkeydown!) to prevent invalid characters from being displayed, but not stopping control
    /// sequences like Ctrl-A from being typed.
    /// </summary>
    private static string DisableInvalidCharacters => @"if(/[^0-9.,$]/gm.test(event.key)) { event.preventDefault(); }";

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", ReadOnlyCss, CssClass);

    private string Value { get; set; } = "";

    private async Task HandleChange(ChangeEventArgs args)
    {
        if(Property.Formatter.TryParse(args.Value?.ToString(), out var value)) {
            Property.SetValue(Model, value);
            var newValue = Property.Formatter.Format(value);
            if(newValue == Value) {
                // rare case where Value property doesn't change because of formatting issues. E.g.
                // 123.45 written as 1,2,3.45 won't trigger Value change refresh.
                Value = $" {newValue}"; // stringly different but minimize UI flicker
                StateHasChanged();
                await Task.Delay(1); // let the UI update
            }
            Value = newValue;
            StateHasChanged();
            var validation = ValidateProperty();
            await InvokeOnChangeAsync(new ChangeEventArgs { Value = value });
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
}
