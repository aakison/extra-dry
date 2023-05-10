#nullable enable

namespace Sample.Client;

#pragma warning disable CA1822 // Mark members as static b/c members are used by reflection menus.

public class MainMenu {

    [Navigation(Icon = "register")]
    public string Register => $"/dummy/a";

    [Navigation(Icon = "contents")]
    public string Contents => $"/contents";

    [Navigation(Icon = "companies")]
    public string Companies => $"/companies/list";

    [Navigation(Icon = "sectors")]
    public string Sectors => $"/sectors";

    [Navigation(Icon = "dummy")]
    public string Dummy3 => $"/dummy/3";

    [Navigation(Icon = "dummy")]
    public string Dummy4 => $"/dummy/4";

    [Navigation(Icon = "dummy")]
    public string Dummy5 => $"/dummy/5";

}
