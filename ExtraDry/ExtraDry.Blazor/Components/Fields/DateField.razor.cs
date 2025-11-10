using System.Runtime.Serialization;
using ExtraDry.Blazor.Components.Formatting;

namespace ExtraDry.Blazor.Components;

/// <summary>
/// Represents a text field that can be single line or multi-line depending on the MaxLength property.
/// </summary>
public partial class DateField<TValue> : FieldBase<TValue>
{

    /// <summary>
    /// The size of the field.  Default PropertySize.Small
    /// </summary>
    [Parameter]
    public override PropertySize Size { get; set; } = PropertySize.Small;

    /// <summary>
    /// Provide an explicit date formatter instead of the default one for the type.  Default
    /// formatters are provided for common date types.
    /// </summary>
    [Parameter]
    public IValueFormatter? Formatter { get; set; }

    protected override void OnParametersSet()
    {
        if(ResolvedFormatter == null) {
            ResolvedFormatter = Formatter ?? typeof(TValue) switch {
                Type t when t == typeof(DateTime) => new DateTimeFormatter(),
                Type t when t == typeof(DateTime?) => new NullableDateTimeFormatter(),
                Type t when t == typeof(DateOnly) => new DateOnlyFormatter(),
                Type t when t == typeof(DateOnly?) => new NullableDateOnlyFormatter(),
                _ => new IdentityFormatter(),
            };
            DisplayValue = ResolvedFormatter.Format(null);
        }
        if(!inputing) {
            DisplayValue = ResolvedFormatter.Format(Value);
        }
        base.OnParametersSet();
    }


    // BaseField doesn't handle conversion from string to TValue, so we do it here.
    protected override async Task NotifyChange(ChangeEventArgs args)
    {
        DisplayValue = (string)(args.Value ?? "");
        if(ResolvedFormatter.TryParse(DisplayValue, out var result)) {
            args.Value = (TValue)result!;
        }
        else {
            // Set arg/model value *and* display value to last valid value
            args.Value = Value;
            DisplayValue = ResolvedFormatter.Format(Value);
        }
        await base.NotifyChange(args);
        Console.WriteLine($"NotifyChange: Exiting Value: {args.Value}");
        inputing = false;
    }

    protected override async Task NotifyInput(ChangeEventArgs args)
    {
        Console.WriteLine($"NotifyInput: Entering DisplayValue: {args.Value}, Type: {args.Value.GetType().Name}");
        DisplayValue = (string)(args.Value ?? "");
        if(ResolvedFormatter.TryParse(DisplayValue, out var result)) {
            Console.WriteLine($"Casting result {result?.GetType().Name} to {typeof(TValue).Name}");
            args.Value = (TValue)result!;
        }
        else {
            // Set arg value (and therefore model value) to last valid value
            args.Value = Value;
        }
        await base.NotifyInput(args);
        Console.WriteLine($"NotifyInput: Exiting Value: {args.Value}");
    }

    // Formatting lock so that we don't overwrite user input while they are typing, hooks to focus/blur events.
    private void LockFormatting(FocusEventArgs _)
    {
        inputing = true;
    }

    private void UnlockFormatting(FocusEventArgs _)
    {
        inputing = false;
    }

    private bool inputing;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "date", ReadOnlyCss, IsValidCss, CssClass);

    private string DisplayValue { get; set; } = "";

    private IValueFormatter ResolvedFormatter { get; set; } = null!;

}
