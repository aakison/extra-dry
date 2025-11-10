using ExtraDry.Blazor.Components.Formatting;
using ExtraDry.Blazor.Models.InputValueFormatters;

namespace ExtraDry.Blazor.Components;

/// <summary>
/// A DRY wrapper around a date input field. Prefer the use of DryField instead
/// of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryDateField<TModel> : DryFieldBase<TModel> where TModel : class
{

    protected override void OnParametersSet()
    {
        if(Model == null || Property == null) {
            return;
        }
        Formatter = (Property.PropertyType, Property.AllowsNull) switch {
            (Type t, false) when t == typeof(DateTime) => new DateTimeFormatter(),
            (Type t, true) when t == typeof(DateTime) => new NullableDateTimeFormatter(),
            (Type t, false) when t == typeof(DateOnly) => new DateOnlyFormatter(),
            (Type t, true) when t == typeof(DateOnly) => new NullableDateOnlyFormatter(),
            _ => null,
        };
        Value = Property.GetValue(Model);
    }

    private IValueFormatter? Formatter { get; set; }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "date", ReadOnlyCss, CssClass);

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
