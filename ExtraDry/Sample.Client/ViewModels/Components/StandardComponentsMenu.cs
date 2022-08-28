namespace Sample.Client;

#pragma warning disable CA1822 // Mark members as static b/c members are used by reflection menus.

public class StandardComponentsMenu {

    [Navigation(Icon = "gravatar")]
    public string Gravatar => $"/components/standard/gravatar";

    [Navigation(Icon = "tri-check")]
    public string TriCheck => $"/components/standard/tri-check";

    [Navigation(Icon = "icons")]
    public string Icon => $"/components/standard/icon";

    [Navigation(Icon = "mini-card")]
    public string MiniCard => $"/components/standard/mini-card";

    [Navigation(Icon = "code-block")]
    public string CodeBlock => $"/components/standard/code-block";

    [Navigation(Icon = "flexi-select")]
    public string FlexiSelect => $"/components/standard/flexi-select";

}
