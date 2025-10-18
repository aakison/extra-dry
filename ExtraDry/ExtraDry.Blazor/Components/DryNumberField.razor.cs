using ExtraDry.Blazor.Models.InputValueFormatters;

namespace ExtraDry.Blazor.Components;

/// <summary>
/// A DRY wrapper around a number input field. Prefer the use of DryField instead
/// of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryNumberField<TModel> : DryFieldBase<TModel> where TModel : class
{

    /// <summary>
    /// The minimum value allowed. If not set, uses double.MinValue.
    /// </summary>
    [Parameter]
    public double? Min { get; set; }

    protected double ResolvedMin => Min ?? double.MinValue;

    /// <summary>
    /// The maximum value allowed. If not set, uses double.MaxValue.
    /// </summary>
    [Parameter]
    public double? Max { get; set; }

    protected double ResolvedMax => Max ?? double.MaxValue;

    private InputValueFormatter Formatter { get; set; } = null!;

    protected override void OnParametersSet()
    {
        Formatter = Property.Formatter;
        if(Model == null || Property == null) {
            return;
        }
        var objValue = Property.GetValue(Model);
        Value = Formatter.Format(objValue);
    }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "number", ReadOnlyCss, CssClass);

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
                await Task.Yield(); // let the UI update
            }
            Value = newValue;
            StateHasChanged();
            var isValid = Validator.Validate(value);
            await OnChange.InvokeAsync(new ChangeEventArgs { Value = value });
            await OnValidate.InvokeAsync(new ValidationEventArgs { IsValid = isValid, MemberName = Property.Property.Name, Message = Validator.Message });
        }
        else {
            var validation = new ValidationEventArgs() {
                IsValid = false,
                MemberName = Property.Property.Name,
                Message = "Invalid number format."
            };
            await OnValidate.InvokeAsync(validation);
        }
    }

    private async Task HandleInput(ChangeEventArgs args)
    {
        var stringValue = args.Value?.ToString() ?? "";
        if(Formatter.TryParse(stringValue, out var value)) {
            Property.SetValue(Model, value);
            await OnInput.InvokeAsync(new ChangeEventArgs { Value = value });
        }
        else {
            // Invalid, but don't set
        }
    }
}
