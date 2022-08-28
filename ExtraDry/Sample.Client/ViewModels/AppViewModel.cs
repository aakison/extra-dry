namespace Sample.Client;

public class AppViewModel {

    public string Title => "Yellow Jacket";

    public string Thumbnail => "waffle";

    public ModulesViewModel Modules { get; set; } = new();

}

public class ModulesViewModel
{
    [Navigation(Icon = "components-module", Subtitle = "Preview and test both standard and DRY components.")]
    public string BlazorComponents => "/components/standard/gravatar";

    [Navigation(Icon = "companies-module", Subtitle = "Administative functionality for a sample app.  With additional information to break the UI and test ellipsis based word wrapping.")]
    public string CompanyModule => "/companies";

    public ComponentsModule Components { get; } = new();

    public CompaniesModule Companies { get; } = new();

}

public class ComponentsModule {

    [Navigation(Icon = "components")]
    public string StandardComponents => "/components/standard/gravatar";

    [Navigation(Icon = "companies")]
    public string DryComponents => "/components/dry/dry-mini-card";

    public StandardComponentsMenu Standard { get; } = new();

    public DryComponentsMenu Dry { get; } = new();

}

public class CompaniesModule {

    public MainMenu Main { get; } = new();

}
