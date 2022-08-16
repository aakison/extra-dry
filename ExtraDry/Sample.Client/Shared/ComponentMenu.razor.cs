#nullable enable

using ExtraDry.Core;
using Microsoft.AspNetCore.Components;
using System;

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

    public class MainMenu {
        [Navigation(Icon = "register")]
        public string Register => $"/dummy/a";

        [Navigation(Icon = "contents")]
        public string Contents => $"/contents";

        [Navigation(Icon = "companies")]
        public string Companies => $"/companies";

        [Navigation(Icon = "sectors")]
        public string Sectors => $"/sectors";

        [Navigation(Icon = "dummy")]
        public string Dummy3 => $"/dummy/3";

        [Navigation(Icon = "dummy")]
        public string Dummy4 => $"/dummy/4";

        [Navigation(Icon = "dummy")]
        public string Dummy5 => $"/dummy/5";
    }

    public class StandardComponentsMenu
    {
        [Navigation(Icon = "icons")]
        public string Icon => $"/components/standard/icon";

        [Navigation(Icon = "gravatar")]
        public string Gravatar => $"/components/standard/gravatar";

        [Navigation(Icon = "tri-check")]
        public string TriCheck => $"/components/standard/tri-check";

        [Navigation(Icon = "mini-card")]
        public string MiniCard => $"/components/standard/mini-card";

        [Navigation(Icon = "code-block")]
        public string CodeBlock => $"/components/standard/code-block";

        [Navigation(Icon = "mini-dialog")]
        public string MiniDialog => $"/components/standrar/mini-dialog";
    }

    public class DryComponentsMenu
    {
        [Navigation(Icon = "mini-card")]
        public string DryMiniCard => $"/components/dry/dry-mini-card";
    }

}
