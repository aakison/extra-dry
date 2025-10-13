namespace ExtraDry.Blazor.Components;

/// <summary>
/// A DRY wrapper around a text input field. Prefer the use of <see cref="DryField{T}" /> instead
/// of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryTextField<T> : DryFieldBase<T> where T : class
{
    protected override void OnParametersSet()
    {
        if(Model == null || Property == null) {
            return;
        }
        Value = Property.DisplayValue(Model);
    }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "text", ReadOnlyCss, CssClass);

    private string Value { get; set; } = "";

    private async Task HandleChange(ChangeEventArgs args)
    {
        if(Property == null || Model == null) {
            return;
        }
        var value = args.Value;
        Property.SetValue(Model, value);
        var valid = ValidateProperty();

        await OnChange.InvokeAsync(args);
        await InvokeOnValidationAsync(valid);
    }

    private async Task HandleInput(ChangeEventArgs args)
    {
        if(Property == null || Model == null) {
            return;
        }
        var value = args.Value;
        Property.SetValue(Model, value);
        await OnInput.InvokeAsync(args);
    }
}
