namespace ExtraDry.Blazor;

public class Menu : ISubjectViewModel
{
    /// <summary>
    /// The optional icon to render when creating the menu link. Icon details are looked up in by
    /// key the `Theme` Icons collection, or can be a URL.
    /// </summary>
    public string Icon { get; set; } = "";

    /// <summary>
    /// The title that is displayed for the navigation link.
    /// </summary>
    public string Title { get; set; } = "";

    /// <summary>
    /// The optional subtitle for the displayed link, typically only used for top-most navigations.
    /// </summary>
    public string Subtitle { get; set; } = "";

    /// <summary>
    /// The order that the Navigation should be displayed in. If not provided, then defaults to
    /// order declared.
    /// </summary>
    public int? Order { get; set; }

    /// <summary>
    /// An navigation link for the menu. This link is always shown to users in browsers on hover
    /// but might not be the target. If the menu defines a NavAction, that will be used instead.
    /// </summary>
    public string? NavLink { get; set; }

    public Action? NavAction { get; set; }

    /// <summary>
    /// An optional regex string that determines if a CSS class of "active" is applied to the
    /// navigation link based on the current URI.
    /// </summary>
    public string? ActiveMatch { get; set; }

    /// <summary>
    /// Menus, when there are many, can be organized into separate groups.
    /// </summary>
    public string Group { get; set; } = string.Empty;

    /// <summary>
    /// A set of menus that are dependant or refine the current menu. Typically used as a sub-menu
    /// that is revealed based on a parent-menu being selected.
    /// </summary>
    public Menu[] Children { get; set; } = [];

    /// <summary>
    /// Not used.
    /// </summary>
    public string Code => string.Empty;

    /// <summary>
    /// Not used.
    /// </summary>
    public string Caption => string.Empty;

    /// <summary>
    /// Not used.
    /// </summary>
    public string Description => string.Empty;

    public bool IsActive(string url)
    {
        if(ActiveMatch != null) {
            return url.Contains(ActiveMatch, StringComparison.OrdinalIgnoreCase);
        }
        else if(NavLink != null) {
            return url.Contains(NavLink, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public Menu? ActiveMenu(string url, int depth = 0)
    {
        if(depth == 0) {
            return this;
        }
        var active = Children.SingleOrDefault(e => e.IsActive(url));
        return active == null ? active : active.ActiveMenu(url, depth - 1);
    }

    public virtual Task ExecuteAsync(NavigationManager navigation)
    {
        NavAction?.Invoke();
        if(NavLink != null) {
            if(NavLink.StartsWith('#')) {
                var bits = navigation.Uri.Split('#', 2);
                navigation.NavigateTo($"{bits[0]}{NavLink}");
            }
            else {
                navigation.NavigateTo(NavLink);
            }
        }
        return Task.CompletedTask;
    }
}
