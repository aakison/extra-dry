using Microsoft.AspNetCore.Components.Routing;

namespace ExtraDry.Blazor;

/// <summary>
/// The DryNavigation component consistently displays navigation menus from either a local list of 
/// menus or a global menu heirarchy.
/// </summary>
public partial class DryNavigation : ComponentBase, IExtraDryComponent, IDisposable {

    /// <summary>
    /// The current menu that drives the navigation.  The actual navigation items are the Children
    /// property of the indicate menu.  Optionally, if the MenuDepth is greater than 0, then the
    /// navigation will be aligned to the indicated depth along the currently active menu path.
    /// </summary>
    [Parameter, EditorRequired]
    public Menu Menu { get; set; } = null!;

    /// <summary>
    /// The optional depth of menus within Menu for which the navigation is displayed.
    /// </summary>
    [Parameter]
    public int MenuDepth { get; set; }

    /// <summary>
    /// Event called whenever a navigation occurs within this navigation component.
    /// </summary>
    [Parameter]
    public EventCallback<Menu> OnNavigated { get; set; }

    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    public async Task NavigateToPreviousAsync()
    {
        var previous = DisplayMenus.Reverse()
            .SkipWhile(e => !e.IsActive(Navigation.Uri))
            .SkipWhile(e => e.IsActive(Navigation.Uri))
            .FirstOrDefault() ?? DisplayMenus.FirstOrDefault();
        if(previous is not null) {
            await DoItemClick(previous);
        }
    }

    public async Task NavigateToNextAsync()
    {
        var next = DisplayMenus
            .SkipWhile(e => !e.IsActive(Navigation.Uri))
            .SkipWhile(e => e.IsActive(Navigation.Uri))
            .FirstOrDefault() ?? DisplayMenus.LastOrDefault();
        if(next is not null) {
            await DoItemClick(next);
        }
    }

    public void Dispose()
    {
        Navigation.LocationChanged -= Navigated;
    }

    protected override void OnInitialized()
    {
        Navigation.LocationChanged += Navigated;
    }

    protected string CssClasses => DataConverter.JoinNonEmpty(" ", "dry-navigation", CssClass);

    private async Task DoItemClick(Menu item)
    {
        await item.ExecuteAsync(Navigation);
        await OnNavigated.InvokeAsync(item);
    }

    private void Navigated(object? sender, LocationChangedEventArgs args)
    {
        StateHasChanged();
    }

    /// <summary>
    /// Based on the currently active menus in the hierarchy, the menu that is active at the 
    /// depth of this DryNavigation.
    /// </summary>
    private Menu? DisplayMenu => Menu?.ActiveMenu(Navigation.Uri, MenuDepth);

    private Menu[] DisplayMenus => DisplayMenu?.Children ?? Array.Empty<Menu>();

    /// <summary>
    /// If any of the menu items has a group declared, then render with groups.
    /// </summary>
    private bool HasNavigationGroups => DisplayMenus.Any(e => !string.IsNullOrWhiteSpace(e.Group));

    /// <summary>
    /// The list of groups that the items should be sorted by.
    /// </summary>
    private IEnumerable<string> NavigationGroups => DisplayMenus.Select(e => e.Group).Distinct();

    /// <summary>
    /// When not grouped, the sorted list of items by their declared Order. If no declared Order
    /// then sort them as if they had an Order of 10,000 (aligned with Microsoft ordering advice)
    /// and then by the order they were declared in the Items collection.
    /// </summary>
    /// <remarks>
    /// Relies on OrderBy providing a stable sort.
    /// </remarks>
    private IEnumerable<Menu> SortedItems => DisplayMenus.OrderBy(e => e.Order ?? 10_000);

    /// <summary>
    /// When grouped, the sorted list of items within a group.  Constrained as SortedItems above.
    /// </summary>
    private IEnumerable<Menu> SortedInGroup(string group) => DisplayMenus.Where(e => e.Group == group).OrderBy(e => e.Order ?? 10_000);

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

}
