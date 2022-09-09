#nullable enable

using Microsoft.AspNetCore.Components;

namespace Sample.Client.Shared;

public sealed partial class ComponentMenu : ComponentBase, IDisposable {

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        NavigationManager.LocationChanged += NavigationManager_LocationChanged;
        SetComponentMenu(NavigationManager.Uri);
    }

    private void NavigationManager_LocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
    {
        SetComponentMenu(e.Location);
    }

    private void SetComponentMenu(string location)
    {
        Console.WriteLine(location);
        if(location.Contains("components/standard", StringComparison.OrdinalIgnoreCase)) {
            CurrentMenu = new StandardComponentsMenu();
        }
        else if(location.Contains("components/dry", StringComparison.OrdinalIgnoreCase)) {
            CurrentMenu = new DryComponentsMenu();
        }
        else if(location.Contains("components/special", StringComparison.OrdinalIgnoreCase)) {
            CurrentMenu = new SpecialComponentsMenu();
        }
        else {
            CurrentMenu = new MainMenu();
        }
        StateHasChanged();
    }

    public void Dispose()
    {
        if(NavigationManager != null) {
            NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
        }
    }

    private object CurrentMenu { get; set; } = new object();

}
