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

    public IconInfo[] Icons { get; } = new IconInfo[] {
        // Inherited - Can override default icons.
        new IconInfo("search", "/img/glyphs/magnifying-glass-light.svg", "Search", "glyph"),
        new IconInfo("select", "fas fa-chevron-down"),

        // Body glyphs
        new IconInfo("register", "/img/glyphs/registered-light.svg", "Register", "glyph"),
        new IconInfo("contents", "/img/glyphs/file-lines-light.svg", "Contents", "glyph"),
        new IconInfo("companies", "/img/glyphs/buildings-light.svg", "Companies", "glyph"),
        new IconInfo("company", "/img/glyphs/building-light.svg", "Company", "glyph"),
        new IconInfo("dummy", "/img/glyphs/briefcase-light.svg", "Placeholder", "glyph"),
        new IconInfo("sectors", "/img/glyphs/wrench-light.svg", "Sectors", "glyph"),
        new IconInfo("icons", "/img/glyphs/icons-light.svg", "Icons", "glyph"),
        new IconInfo("code-block", "/img/glyphs/code-light.svg", "Code Block", "glyph"),
        new IconInfo("mini-card", "/img/glyphs/address-card-light.svg", "Icons", "glyph"),
        new IconInfo("mini-dialog", "/img/glyphs/window-maximize-light.svg", "Icons", "glyph"),
        new IconInfo("gravatar", "/img/glyphs/circle-user-light.svg", "Icons", "glyph"),
        new IconInfo("tri-check", "/img/glyphs/square-check-light.svg", "Icons", "glyph"),
        new IconInfo("tri-switch", "/img/glyphs/toggle-on-light.svg", "Icons", "glyph"),
        new IconInfo("flexi-select", "/img/glyphs/list-radio-light.svg", "Icons", "glyph"),
        new IconInfo("combo-box", "/img/glyphs/square-caret-down-light.svg", "Icons", "glyph"),
        new IconInfo("button", "/img/glyphs/square-bolt-light.svg", "Icons", "glyph"),
        new IconInfo("error-boundary", "/img/glyphs/ban-bug-light.svg", "Icons", "glyph"),
        new IconInfo("theme", "/img/glyphs/pen-swirl-light.svg", "Icons", "glyph"),
        new IconInfo("navigation", "/img/glyphs/compass-sharp-light.svg", "Icons", "glyph"),
        new IconInfo("reveal", "/img/glyphs/expand-light.svg", "Reveal", "glyph"),
        new IconInfo("suspense", "/img/glyphs/arrow-rotate-left-light.svg", "Suspense", "glyph"),
        new IconInfo("edit", "fas fa-edit"),
        new IconInfo("plus", "fas fa-plus"),
        new IconInfo("filter", "fas fa-filter"),
        new IconInfo("expand", "fas fa-expand-alt"),
        new IconInfo("compress", "fas fa-compress-alt"),
        new IconInfo("chevron-down", "fas fa-chevron-down"),

        new IconInfo("option-checked", "/img/glyphs/square-check-solid.svg", "Checked", "small-icon"),
        new IconInfo("option-unchecked", "/img/glyphs/square-regular.svg", "Unchecked", "small-icon"),
        new IconInfo("option-indeterminate", "/img/glyphs/square-minus-duotone.svg", "Indeterminate", "small-icon"),

        // Header glyphs
        new IconInfo("modules", "/img/glyphs/grid-solid.svg", "Select Module", "glyph"),
        new IconInfo("alerts", "/img/glyphs/bell-regular.svg", "Alerts", "glyph"),
        new IconInfo("newItem", "/img/glyphs/hexagon-plus-regular.svg", "Add Item", "glyph"),

        // Hero icons
        new IconInfo("assets", "/img/icons/asset-icon.svg", "Assets", "icon"),
        new IconInfo("logo", "/img/icons/logo.svg", "Site Logo", "icon", SvgRenderType.Reference),
        new IconInfo("companies-module", "/img/icons/companies-icon.svg", "Company Module", "icon"),
        new IconInfo("components-module", "/img/icons/components-icon.svg", "Component Module", "icon"),
        new IconInfo("standard-components", "/img/icons/components-icon-2.svg", "Standard Components", "icon", SvgRenderType.Inline),
        new IconInfo("dry-components", "/img/icons/components-icon-3.svg", "DRY Components", "icon", SvgRenderType.Inline),
        new IconInfo("special-components", "/img/icons/components-icon-1.svg", "Special Components", "icon", SvgRenderType.Inline),
        new IconInfo("api-module", "/img/icons/api-icon.svg", "Integration APIs", "icon"),
        new IconInfo("overdue", "/img/icons/overdue.svg", "Overdue", "icon", SvgRenderType.Document),

        //Loader icons
        new IconInfo("loader-error", "fa fa-circle-exclamation"),
        new IconInfo("loader-timeout", "fa fa-rotate-right"),

    };


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
