namespace Sample.Client;

public class AppViewModel {

    public string Title => "Yellow Jacket";

    public string Thumbnail => "waffle";

    public ModulesViewModel Modules { get; set; } = new();

}

public class ModulesViewModel
{
    [Navigation(Icon = "components-module", 
        ActiveMatch = "/components",
        Subtitle = "Preview and test both standard and DRY components.")]
    public string BlazorComponents => "/components/standard/gravatar";

    [Navigation(Icon = "companies-module", 
        ActiveMatch = "/companies",
        Subtitle = "Administative functionality for a sample app.  With additional information to break the UI and test ellipsis based word wrapping.")]
    public string CompanyModule => "/companies";

    [Navigation(Title = "Integration APIs", 
        Icon = "api-module",
        Subtitle = "A set of APIs for programmatic integration with other applications.")]
    public string SwaggerModule => "/swagger";

    public ComponentsModule Components { get; } = new();

    public CompaniesModule Companies { get; } = new();

}

public class ComponentsModule {

    [Navigation(Icon = "components")]
    public string StandardComponents => "/components/standard/gravatar";

    [Navigation(Icon = "companies")]
    public string DryComponents => "/components/dry/dry-mini-card";

    [Navigation(Icon = "companies")]
    public string SpecialComponents => "/components/special/theme";

    public StandardComponentsMenu Standard { get; } = new();

    public DryComponentsMenu Dry { get; } = new();

    public SpecialComponentsMenu Special { get; } = new();

}

public class CompaniesModule {

    public MainMenu Main { get; } = new();

}
