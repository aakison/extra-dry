using ExtraDry.Blazor.Components.Formatting;
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

    protected override void OnParametersSet()
    {
        if(Model == null || Property == null) {
            return;
        }
        Formatter = (Property.PropertyType, Property.AllowsNull) switch {
            (Type t, false) when t == typeof(int) => new IntFormatter(),
            (Type t, true) when t == typeof(int) => new NullableIntFormatter(),
            (Type t, false) when t == typeof(double) => new DoubleFormatter(),
            (Type t, true) when t == typeof(double) => new NullableDoubleFormatter(),
            (Type t, false) when t == typeof(decimal) => new DecimalFormatter(),
            (Type t, true) when t == typeof(decimal) => new NullableDecimalFormatter(),
            _ => null,
        };
        Value = Property.GetValue(Model);
    }

    private INumberFormatter? Formatter { get; set; }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "number", ReadOnlyCss, CssClass);

    private object? Value { get; set; }

    private async Task HandleChange(ChangeEventArgs args)
    {
        Property.SetValue(Model, args.Value);
        await OnChange.InvokeAsync(args);
    }

    private async Task HandleInput(ChangeEventArgs args)
    {
        Property.SetValue(Model, args.Value);
        await OnInput.InvokeAsync(args);
    }

    private async Task HandleValidate(ValidationEventArgs args)
    {
        await OnValidate.InvokeAsync(args);
    }
}
