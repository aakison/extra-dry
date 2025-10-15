namespace ExtraDry.Blazor.Components;

/// <summary>
/// A DRY wrapper around a text input field. Prefer the use of DryField instead
/// of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryTextField<TModel> : DryFieldBase<TModel> where TModel : class
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
        var value = args.Value;
        Property.SetValue(Model, value);
        await OnChange.InvokeAsync(args);
    }

    private async Task HandleInput(ChangeEventArgs args)
    {
        var value = args.Value;
        Property.SetValue(Model, value);
        await OnInput.InvokeAsync(args);
    }

    private async Task HandleValidate(ValidationEventArgs args)
    {
        await OnValidate.InvokeAsync(args);
    }
}
