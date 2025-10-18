using ExtraDry.Blazor.Components.Formatting;

namespace ExtraDry.Blazor.Components;

/// <summary>
/// Represents a number field that uses an HTML input type="number".
/// </summary>
public partial class NumberField<TValue> : FieldBase<TValue>
{

    /// <summary>
    /// The minimum value allowed.
    /// </summary>
    [Parameter]
    public double Min { get; set; } = double.MinValue;

    /// <summary>
    /// The maximum value allowed.
    /// </summary>
    [Parameter]
    public double Max { get; set; } = double.MaxValue;

    /// <summary>
    /// The size of the field.
    /// </summary>
    [Parameter]
    public override PropertySize Size { get; set; } = PropertySize.Medium;

    /// <summary>
    /// Only allow digits, commas, periods and dollar signs. Injected into onkeypress event (not
    /// onkeydown!) to prevent invalid characters from being displayed, but not stopping control
    /// sequences like Ctrl-A from being typed.
    /// </summary>
    private static string DisableInvalidCharacters => @"if(/[^0-9.\-,$]/gm.test(event.key)) { event.preventDefault(); }";

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", ReadOnlyCss, IsValidCss, CssClass);

    private string DisplayValue { get; set; } = "";

    protected override void OnInitialized()
    {
        Formatter = typeof(TValue) switch {
            Type t when t == typeof(int) => new IntFormatter(),
            Type t when t == typeof(int?) => new NullableIntFormatter(),
            Type t when t == typeof(double) => new DoubleFormatter(),
            Type t when t == typeof(double?) => new NullableDoubleFormatter(),
            Type t when t == typeof(decimal) => new DecimalFormatter(),
            Type t when t == typeof(decimal?) => new NullableDecimalFormatter(),
            _ => new IdentityFormatter(),
        };
        DisplayValue = Formatter.Format(null);
        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        if(!inputing) {
            DisplayValue = Formatter.Format(Value);
        }
        base.OnParametersSet();
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


    // BaseField doesn't handle conversion from string to TValue, so we do it here.
    protected override async Task NotifyChange(ChangeEventArgs args)
    {
        DisplayValue = (string)(args.Value ?? "");
        if(Formatter.TryParse(DisplayValue, out var result)) {
            args.Value = (TValue)result!;
        }
        else {
            // Set arg/model value *and* display value to last valid value
            args.Value = Value;
            DisplayValue = Formatter.Format(Value);
        }
        await base.NotifyChange(args);
        inputing = false;
    }

    protected override async Task NotifyInput(ChangeEventArgs args)
    {
        Console.WriteLine($"NotifyInput: Entering DisplayValue: {DisplayValue}");
        DisplayValue = (string)(args.Value ?? "");
        if(Formatter.TryParse(DisplayValue, out var result)) {
            args.Value = (TValue)result!;
        }
        else {
            // Set arg value (and therefore model value) to last valid value
            args.Value = Value; 
        }
        await base.NotifyInput(args);
        Console.WriteLine($"NotifyInput: Exiting DisplayValue: {DisplayValue}");
    }

    private string Pattern => Formatter.RegexPattern;

    private INumberFormatter Formatter { get; set; } = null!;

}
