using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a text input field.  Prefer the use of <see cref="DryInput{T}"/> 
/// instead of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputText<T> : DryInputBase<T> {

    [Parameter]
    public bool ReadOnly { get; set; }

    protected override void OnParametersSet()
    {
        if(Model == null || Property == null) {
            return;
        }
        Value = Property.DisplayValue(Model);
    }

    private string Placeholder => Property?.Display?.Prompt ?? "";

    private string Icon => Property?.InputFormat?.Icon ?? "";

    private string Affordance => Property?.InputFormat?.Affordance ?? "";

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", ReadOnlyCss, CssClass);

    private string Value { get; set; } = "";

    private async Task InvokeOnChange(ChangeEventArgs args)
    {
        await OnChange.InvokeAsync(args);
    }

    private async Task HandleChange(ChangeEventArgs args)
    {
        if(Property == null || Model == null) {
            return;
        }
        var value = args.Value;
        Property.SetValue(Model, value);
        var valid = ValidateProperty();

        await InvokeOnChangeAsync(value);
        await InvokeOnValidationAsync(valid);
    }
}
