namespace ExtraDry.Blazor;

/// <summary>
/// Represents a simple styled spinning circle which can be used to represent a value or item in a
/// loading state
/// </summary>
public partial class Spinner : ComponentBase, IExtraDryComponent
{
    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="IndicatorSize" />
    [Parameter]
    public IndicatorSize Size { get; set; } = IndicatorSize.Standard;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    public static string GetSpinnerSizeCssClass(IndicatorSize size) => size switch {
        IndicatorSize.Standard => "",
        IndicatorSize.Small => "small",
        IndicatorSize.Large => "large",
        _ => throw new NotImplementedException(),
    };

    private string CssClasses => DataConverter.JoinNonEmpty(" ", CssClass, "spinner", GetSpinnerSizeCssClass(Size));
}
