namespace Sample.Client;

#pragma warning disable CA1822 // Mark members as static b/c members are used by reflection menus.

public class DryComponentsMenu {
    [Navigation(Icon = "mini-card")]
    public string DryMiniCard => $"/components/dry/dry-mini-card";
}
