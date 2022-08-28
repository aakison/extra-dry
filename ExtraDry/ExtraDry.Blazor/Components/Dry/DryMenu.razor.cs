#nullable enable

using Microsoft.AspNetCore.Components.Routing;

namespace ExtraDry.Blazor;

/// <summary>
/// Represents a type of navigation that is controlled using a menu.
/// Uses same `NavigationAttribute`s as `DryNavigation` but renders inside a MiniDialog.
/// </summary>
public partial class DryMenu : DryViewModelComponentBase, IExtraDryComponent, IDisposable {

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="MiniDialog.AnimationDuration" />
    [Parameter]
    public int AnimationDuration { get; set; } = 100;

    /// <summary>
    /// The content of the menu is the clickable area to open the menu.
    /// Typically an image, text div, or MiniCard.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = null!;

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    /// <summary>
    /// Dispose of navigation resources.
    /// </summary>
    public void Dispose()
    {
        Navigation.LocationChanged -= Navigated;
    }

    /// <summary>
    /// Attach to navigation resources.
    /// </summary>
    protected override void OnInitialized()
    {
        Navigation.LocationChanged += Navigated;
    }

    private MiniDialog MiniDialog { get; set; } = null!;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-menu", CssClass);

    private async void DoClick(MouseEventArgs _)
    {
        await MiniDialog.ShowAsync();
    }

    private async void DoItemClick(NavigationDescription item)
    {
        await MiniDialog.HideAsync();
        await item.ExecuteAsync();
    }

    private void Navigated(object? sender, LocationChangedEventArgs args)
    {
        StateHasChanged();
    }

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

}
