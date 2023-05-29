namespace ExtraDry.Blazor;
/// <summary>
/// Represents a simple styled spinning circle which can be used to represent
/// a value or item in a loading state
/// </summary>
public partial class Spinner : ComponentBase, IExtraDryComponent {
    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public SpinnerSize Size { get; set; } = SpinnerSize.Standard;

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    public static string GetSpinnerSizeCssClass(SpinnerSize size) => size switch {
        SpinnerSize.Standard => "",
        SpinnerSize.Small => "small",
        SpinnerSize.Large => "large",
        _ => throw new NotImplementedException(),
    };

    private string CssClasses => DataConverter.JoinNonEmpty(" ", CssClass, "spinner", GetSpinnerSizeCssClass(Size));
}
