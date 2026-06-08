using ExtraDry.Blazor.Components.Formatting;
using ExtraDry.Core.Formatters;

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
    /// Provide an explicit number formatter instead of the default one for the type.  Default
    /// formatters are provided for common numeric types.
    /// </summary>
    [Parameter]
    public IValueFormatter? Formatter { get; set; }

    /// <summary>
    /// Only allow digits, commas, periods, dollar signs, and percentage signs. Injected into onkeypress event (not
    /// onkeydown!) to prevent invalid characters from being displayed, but not stopping control
    /// sequences like Ctrl-A from being typed.
    /// </summary>
    private static string DisableInvalidCharacters => @"if(/[^0-9.\-,$%]/gm.test(event.key)) { event.preventDefault(); }";

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "number", ReadOnlyCss, IsValidCss, CssClass);

    private string DisplayValue { get; set; } = "";

    protected override void OnParametersSet()
    {
        ResolvedFormatter ??= Formatter ?? typeof(TValue) switch {
            Type t when t == typeof(int) => new IntFormatter(),
            Type t when t == typeof(int?) => new NullableIntFormatter(),
            Type t when t == typeof(double) => new DoubleFormatter(),
            Type t when t == typeof(double?) => new NullableDoubleFormatter(),
            Type t when t == typeof(decimal) => new DecimalFormatter(),
            Type t when t == typeof(decimal?) => new NullableDecimalFormatter(),
            _ => new IdentityFormatter(),
        };
        if(!inputing) {
            DisplayValue = ResolvedFormatter.Format(Value);
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

    protected override async Task NotifyChange(ChangeEventArgs args)
    {
        inputing = false;
        await base.NotifyChange(args);
    }

    private string Pattern => ResolvedFormatter.RegexPattern;

    private IValueFormatter ResolvedFormatter { get; set; } = null!;

}
