namespace ExtraDry.Blazor;

/// <summary>
/// Provides functionality for users to copy complex snippets of text from the system, such as
/// URLs or crypto-keys. Renders as a read-only <c>&lt;pre&gt;</c> block with icon buttons to
/// toggle visibility (when <see cref="IsSecret"/> is <c>true</c>) and copy to clipboard.
/// </summary>
public partial class CopyHelper : ComponentBase, IExtraDryComponent
{
    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The text value to display and copy. This is always the value placed into the clipboard,
    /// regardless of whether the value is currently revealed or masked.
    /// </summary>
    [Parameter, EditorRequired]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// When <c>true</c>, the value is masked by default and a Show/Hide toggle button is
    /// displayed. When <c>false</c>, the value is always shown and the toggle button is hidden.
    /// </summary>
    [Parameter]
    public bool IsSecret { get; set; }

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    private bool IsRevealed { get; set; }

    private string DisplayValue => IsSecret && !IsRevealed
        ? new string('●', Math.Max(Value.Length, 50))
        : Value;

    private string RevealIcon => IsRevealed ? "hide" : "show";

    private string RevealCaption => IsRevealed ? "Hide" : "Show";

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "copy-helper", CssClass);

    private void ToggleReveal()
    {
        IsRevealed = !IsRevealed;
    }

    private async Task CopyToClipboard()
    {
        await Module.InvokeVoidAsync("CopyHelper_CopyToClipboard", Value);
    }

    [Inject]
    private ExtraDryJavascriptModule Module { get; set; } = null!;
}
