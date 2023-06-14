namespace Sample.Client;

#pragma warning disable CA1822 // Mark members as static b/c members are used by reflection menus.

public class StandardComponentsMenu {

    [Navigation(Icon = "gravatar")]
    public string Gravatar => $"/components/standard/gravatar";

    [Navigation(Icon = "tri-check")]
    public string TriCheck => $"/components/standard/tri-check";

    [Navigation(Icon = "tri-switch")]
    public string TriSwitch => $"/components/standard/tri-switch";

    [Navigation(Icon = "icons")]
    public string Icon => $"/components/standard/icon";

    [Navigation(Icon = "combo-box")]
    public string ComboBox => $"/components/standard/combo-box";

    [Navigation(Icon = "mini-card")]
    public string MiniCard => $"/components/standard/mini-card";

    [Navigation(Icon = "code-block")]
    public string CodeBlock => $"/components/standard/code-block";

    [Navigation(Icon = "mini-dialog")]
    public string MiniDialog => $"/components/standard/mini-dialog";

    [Navigation(Icon = "flexi-select")]
    public string FlexiSelect => $"/components/standard/flexi-select";

    [Navigation(Icon = "button")]
    public string Button => $"/components/standard/button";

}
