using ExtraDry.Blazor;
using Microsoft.AspNetCore.Components;

namespace Sample.Client;

public class AppViewModel {

    public AppViewModel(NavigationManager navigationManager) {
        navigation = navigationManager;
    }

    public Menu Menu {
        get {
            menu ??= CreateMenu();
            return menu;
        }
    }
    private Menu? menu;

    private Menu CreateMenu() => new() { 
        Icon = "modules",
        Title = "Yellow Jacket",
        Children = new Menu[] {
            new Menu {
                Icon = "components-module",
                Title = "Blazor Components",
                Subtitle = "Preview and test both standard and DRY components.",
                NavLink = "/components/standard/gravatar",
                ActiveMatch = "/components",
                Children = new Menu[] {
                    new Menu {
                        Icon = "standard-components",
                        Title = "Standard Components",
                        ActiveMatch = "/components/standard",
                        NavLink = "/components/standard/gravatar",
                        Children = new Menu[] {
                            new Menu {
                                Icon = "gravatar",
                                Title = "Gravatar",
                                ActiveMatch = "/components/standard/gravatar",
                                NavLink = "/components/standard/gravatar",
                                NavAction = () => navigation.NavigateTo("/components/standard/gravatar")
                            },
                            new Menu {
                                Icon = "tri-check",
                                Title = "Tri Check",
                                NavLink = "/components/standard/tri-check",
                            },
                            new Menu {
                                Icon = "tri-switch",
                                Title = "Tri Switch",
                                NavLink = "/components/standard/tri-switch",
                            },
                            new Menu {
                                Icon = "icons",
                                Title = "Icons",
                                NavLink = "/components/standard/icon",
                            },
                            new Menu {
                                Icon = "combo-box",
                                Title = "Combo Box",
                                NavLink = "/components/standard/combo-box",
                            },
                            new Menu {
                                Icon = "mini-card",
                                Title = "Mini Card",
                                NavLink = "/components/standard/mini-card",
                            },
                            new Menu {
                                Icon = "code-block",
                                Title = "Code Block",
                                NavLink = "/components/standard/code-block",
                            },
                            new Menu {
                                Icon = "mini-dialog",
                                Title = "Mini Dialog",
                                NavLink = "/components/standard/mini-dialog",
                            },
                            new Menu {
                                Icon = "flexi-select",
                                Title = "Flexi Select",
                                NavLink = "/components/standard/flexi-select",
                            },
                            new Menu {
                                Icon = "button",
                                Title = "Button",
                                NavLink = "/components/standard/button",
                            },
                        },
                    },
                    new Menu {
                        Icon = "standard-components",
                        Title = "Advanced Components",
                        ActiveMatch = "/components/advanced",
                        NavLink = "/components/advanced/reveal",
                        Children = new Menu[] {
                            new Menu {
                                Icon = "suspense",
                                Title = "Suspense",
                                ActiveMatch = "/components/advanced/suspense",
                                NavLink = "/components/advanced/suspense",
                                NavAction = () => navigation.NavigateTo("/components/advanced/suspense")
                            },
                            new Menu {
                                Icon = "reveal",
                                Title = "Reveal",
                                ActiveMatch = "/components/advanced/reveal",
                                NavLink = "/components/advanced/reveal",
                                NavAction = () => navigation.NavigateTo("/components/advanced/reveal")
                            },
                        },
                    },
                    new Menu {
                        Icon = "dry-components",
                        Title = "DRY Components",
                        ActiveMatch = "/components/dry",
                        NavLink = "/components/dry/dry-mini-card",
                        Children = new Menu[] { 
                            new Menu {
                                Icon = "button",
                                Title = "Button",
                                NavLink = "/components/dry/dry-button",
                            },
                            new Menu {
                                Icon = "flexi-select",
                                Title = "Flexi Select",
                                NavLink = "/components/dry/dry-flexi-select",
                            },
                            new Menu {
                                Icon = "mini-card",
                                Title = "Mini Card",
                                NavLink = "/components/dry/dry-mini-card",
                            },
                            new Menu {
                                Icon = "navigation",
                                Title = "Navigation",
                                NavLink = "/components/dry/dry-navigation",
                            },
                        },
                    },
                    new Menu {
                        Icon = "special-components",
                        Title = "Special Components",
                        ActiveMatch = "/components/special",
                        NavLink = "/components/special/theme",
                        Children = new Menu[] {
                            new Menu {
                                Icon = "error-boundary",
                                Title = "Error Boundary",
                                NavLink = "/components/special/dry-error-boundary",
                            },
                            new Menu {
                                Icon = "theme",
                                Title = "Theme",
                                NavLink = "/components/special/theme",
                            },
                        },
                    },
                },
            },
            new Menu {
                Icon = "companies-module",
                Title = "Administration",
                ActiveMatch = "/admin",
                NavLink = "/admin/works/companies/list",
                Subtitle = "Administative functionality for a sample app.  With additional information to break the UI and test ellipsis based word wrapping.",
                Children = new Menu[] {
                    new Menu {
                        Icon = "assets",
                        Title = "Works",
                        ActiveMatch = "/admin/works",
                        NavLink = "/admin/works/companies/list",
                        Children = new Menu[] {
                            new Menu {
                                Icon = "contents",
                                Title = "Contents",
                                NavLink = "/admin/works/contents/list",
                            },
                            new Menu {
                                Icon = "companies",
                                Title = "Companies",
                                ActiveMatch = "/companies",
                                NavLink = "/admin/works/companies/list",
                            },
                            new Menu {
                                Icon = "sectors",
                                Title = "Sectors",
                                NavLink = "/admin/works/sectors/list",
                            },
                        },
                    },
                },
            },
            new Menu {
                Icon = "api-module",
                Title = "Integration APIs",
                Subtitle = "A set of APIs for programmatic integration with other applications.",
                NavLink = "/swagger",
            },
        }
    };

    private readonly NavigationManager navigation;

}
