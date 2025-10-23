namespace ExtraDry.Blazor;

/// <summary>
/// Represents a type of navigation that is controlled using a menu.
/// </summary>
public partial class DryMenu : IExtraDryComponent
{
    /// <inheritdoc cref="DryNavigation.Menu" />
    [Parameter, EditorRequired]
    public Menu Menu { get; set; } = null!;

    /// <inheritdoc cref="DryNavigation.MenuDepth" />
    [Parameter]
    public int MenuDepth { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="MiniDialog.AnimationDuration" />
    [Parameter]
    public int AnimationDuration { get; set; } = 100;

    /// <summary>
    /// The content of the menu is the clickable area to open the menu. Typically an image, text
    /// div, or MiniCard.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = null!;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    private MiniDialog? MiniDialog { get; set; }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-menu", CssClass);

    private async void DoClick(MouseEventArgs _)
    {
        await MiniDialog!.ShowAsync();
    }

    private async Task DoNavigation(Menu menu)
    {
        await MiniDialog!.HideAsync();
    }
}
