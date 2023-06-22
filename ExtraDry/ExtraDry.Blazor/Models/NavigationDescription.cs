namespace ExtraDry.Blazor.Models;

/// <summary>
/// Represents a property that contains a link or a method that performs a navigation.
/// </summary>
[Obsolete("Use Menu instead.")]
public class NavigationDescription : ISubjectViewModel {

    /// <summary>
    /// Create a `NavigationDescription` with a reference to the ViewModel it will execute on and the method to call.
    /// </summary>
    public NavigationDescription(object viewModel, MethodInfo method)
    {
        ViewModel = viewModel;
        Method = method;
        Navigation = Method.GetCustomAttribute<NavigationAttribute>();
        Initialize();
    }

    /// <summary>
    /// Create a `NavigationDescription` which uses a property to determine the href.
    /// </summary>
    public NavigationDescription(object viewModel, PropertyInfo property)
    {
        ViewModel = viewModel;
        Property = property;
        Navigation = Property.GetCustomAttribute<NavigationAttribute>();
        Initialize();
    }

    /// <summary>
    /// The reflected property for the navigation href.
    /// Mutually exlusive with `Method`.
    /// </summary>
    public PropertyInfo? Property { get; private set; }

    /// <summary>
    /// The reflected method for the navigation link to execute.
    /// Mutually exlusive with `Property`.
    /// </summary>
    public MethodInfo? Method { get; private set; }

    /// <summary>
    /// The navigation attribute that defines the property/method as being for navigation.
    /// </summary>
    [Obsolete("Use Menu object instead.")]
    public NavigationAttribute? Navigation { get; private set; }

    /// <summary>
    /// If a group is provided, the name of the group to display the navigation under.
    /// </summary>
    public string? Group { get; set; }

    /// <summary>
    /// The caption of the command, taken from the `NavigationAttribute` if available.
    /// If not, this is inferred from the signature of the `Method` by convention.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// The subtitle of the navigation, taken from the `NavigationAttribute` if available.
    /// </summary>
    public string Subtitle { get; set; } = string.Empty;

    /// <summary>
    /// The optional Key or Uri of the icon to be displayed on links.
    /// This is the Key from the IconInfo set in the Theme.
    /// </summary>
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// The optional order of the navigation item, if omitted then the file order is respected.
    /// Note, however, method based navigations will list before property based navigations.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// The view model that this command is defined as being part of.
    /// Used by `ExecuteAsync` to invoke the command on the correct object instance.
    /// </summary>
    public object ViewModel { get; set; }

    /// <summary>
    /// A Regex which uses the URI to determine if the current navigation is active or inactive.
    /// The active state is set using the CSS Class of 'active' or 'inactive'.
    /// </summary>
    public string? ActiveMatch { get; set; }

    /// <summary>
    /// Indicates if the navigation uses an Href instead of an onclick...
    /// </summary>
    public bool HasHref => Property != null;

    /// <summary>
    /// The href as defined by the `Property` or non-functional placeholder otherwise if command based navigation.
    /// </summary>
    public string Href => Property?.GetValue(ViewModel)?.ToString() ?? "javascript:void(0)";

    /// <inheritdoc /> 
    public string Code => string.Empty;

    /// <inheritdoc /> 
    public string Caption => $"{Title} Navigation";

    /// <inheritdoc /> 
    public string Description => Subtitle;

    /// <summary>
    /// Determines if the navigation is currently a match for the current URI based
    /// on either the entire HREF, or the ActiveMatch Regex.
    /// </summary>
    public bool UriMatch(NavigationManager navigation)
    {
        var relativeUri = navigation.Uri.Remove(0, navigation.BaseUri.Length);
        var match = ActiveMatch == null ? Href.TrimStart('/') : ActiveMatch.TrimStart('/');
        var isMatch = string.IsNullOrWhiteSpace(match) ? 
            string.IsNullOrWhiteSpace(relativeUri) :
            relativeUri?.Contains(match) ?? false;
        return isMatch;
    }

    /// <summary>
    /// Navigates the link as if it were clicked.  
    /// If a property has been supplied, uses the indicated HREF, otherwise executes the link method.
    /// </summary>
    public async Task Navigate(NavigationManager navigation)
    {
        if(HasHref) {
            navigation.NavigateTo(Href);
        }
        else {
            await ExecuteAsync();
        }
    }

    /// <summary>
    /// Executes the underlying method with the provided arguments, ensuring that the proper number of arguments are provided.
    /// </summary>
    public async Task ExecuteAsync()
    {
        if(Method != null) {
            var result = Method.Invoke(ViewModel, null);
            if(result is Task task) {
                await task;
            }
        }
    }

    private void Initialize()
    {
        Title = Navigation?.Title ?? "Title";
        if(string.IsNullOrWhiteSpace(Title)) {
            Title = DefaultName(Method?.Name ?? Property?.Name ?? "");
        }
        Icon = Navigation?.Icon ?? string.Empty;
        Subtitle = Navigation?.Subtitle ?? string.Empty;
        Group = Navigation?.Group;
        Order = Navigation?.Order ?? 0;
        ActiveMatch = Navigation?.ActiveMatch;
    }

    /// <summary>
    /// Determines a default name for the command based on a formatted version of the method name.
    /// </summary>
    private static string DefaultName(string name)
    {
        name = name.Replace("Async", "");
        name = DataConverter.CamelCaseToTitleCase(name);
        return name;
    }


}
