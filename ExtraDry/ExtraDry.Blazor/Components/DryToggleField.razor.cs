namespace ExtraDry.Blazor.Components;

/// <summary>
/// A DRY wrapper around a toggle field (checkbox). Prefer the use of DryField instead
/// of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryToggleField<TModel> : DryFieldBase<TModel> where TModel : class
{
    protected override void OnParametersSet()
    {
        if(Model == null || Property == null) {
            return;
        }
        Value = (bool?)Property.GetValue(Model);
    }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "checkbox", ReadOnlyCss, CssClass);

    private bool? Value { get; set; }

    private async Task HandleChange(ChangeEventArgs args)
    {
        bool? newValue = args.Value as bool?;
        if (!Property.AllowsNull && newValue == null) {
            newValue = false;
        }
        Property.SetValue(Model, newValue);
        Value = newValue;
        StateHasChanged();
        await OnChange.InvokeAsync(args);
    }

    private async Task HandleInput(ChangeEventArgs args)
    {
        bool? newValue = args.Value as bool?;
        if (!Property.AllowsNull && newValue == null) {
            newValue = false;
        }
        Property.SetValue(Model, newValue);
        Value = newValue;
        StateHasChanged();
        await OnInput.InvokeAsync(args);
    }

    private async Task HandleValidate(ValidationEventArgs args)
    {
        await OnValidate.InvokeAsync(args);
    }
}
