namespace Sample.Client;

#pragma warning disable CA1822 // Mark members as static b/c members are used by reflection menus.

public class SpecialComponentsMenu {
    [Navigation(Icon = "dry-error-boundary")]
    public string DryErrorBoundary => $"/components/special/dry-error-boundary";

    [Navigation(Icon = "theme")]
    public string Theme => $"/components/special/theme";
}
