using Microsoft.AspNetCore.Components;

namespace Sample.Spa.Client;

public class AppViewModel(
    NavigationManager navigation)
{

    public string[]? Filter { get; set; }

    public Menu Menu {
        get {
            menu ??= CreateMenu();
            return menu;
        }
    }
    private Menu? menu;

    public IconInfo[] Icons { get; } = [
        // Inherited - override default icons with lighter versions.
        new("search", "/img/glyphs/magnifying-glass-light.svg", "Search", "glyph"),
        new("select", "/img/glyphs/chevron-down-light.svg", "Collapse", "glyph"),
        new("expand", "/img/glyphs/chevron-right-light.svg", "Expand", "glyph"),
        new("collapse", "/img/glyphs/chevron-down-light.svg", "Collapse", "glyph"),
        new("back", "fas fa-chevron-left"),
        new("forward", "fas fa-chevron-right"),

        // Body glyphs
        new("register", "/img/glyphs/registered-light.svg", "Register", "glyph"),
        new("contents", "/img/glyphs/file-lines-light.svg", "Contents", "glyph"),
        new("companies", "/img/glyphs/buildings-light.svg", "Companies", "glyph"),
        new("company", "/img/glyphs/building-light.svg", "Company", "glyph"),
        new("regions", "/img/glyphs/earth-oceania-light.svg", "Regions", "glyph"),
        new("dummy", "/img/glyphs/briefcase-light.svg", "Placeholder", "glyph"),
        new("sectors", "/img/glyphs/wrench-light.svg", "Sectors", "glyph"),
        new("icons", "/img/glyphs/icons-light.svg", "Icons", "glyph"),
        new("code-block", "/img/glyphs/code-light.svg", "Code Block", "glyph"),
        new("mini-card", "/img/glyphs/address-card-light.svg", "Icons", "glyph"),
        new("mini-dialog", "/img/glyphs/window-maximize-light.svg", "Icons", "glyph"),
        new("gravatar", "/img/glyphs/circle-user-light.svg", "Icons", "glyph"),
        new("tri-check", "/img/glyphs/square-check-light.svg", "Icons", "glyph"),
        new("tri-switch", "/img/glyphs/toggle-on-light.svg", "Icons", "glyph"),
        new("flexi-select", "/img/glyphs/list-radio-light.svg", "Icons", "glyph"),
        new("combo-box", "/img/glyphs/square-caret-down-light.svg", "Icons", "glyph"),
        new("button", "/img/glyphs/square-bolt-light.svg", "Icons", "glyph"),
        new("error-boundary", "/img/glyphs/ban-bug-light.svg", "Icons", "glyph"),
        new("theme", "/img/glyphs/pen-swirl-light.svg", "Icons", "glyph"),
        new("file-validator", "/img/glyphs/file-check-light.svg", "Icons", "glyph"),
        new("navigation", "/img/glyphs/compass-sharp-light.svg", "Icons", "glyph"),
        new("reveal", "/img/glyphs/expand-light.svg", "Reveal", "glyph"),
        new("suspense", "/img/glyphs/arrow-rotate-left-light.svg", "Suspense", "glyph"),
        new("employees", "/img/glyphs/users-light.svg", "Employees", "glyph"),
        new("edit", "fas fa-edit"),
        new("plus", "fas fa-plus"),
        new("filter", "fas fa-filter"),
        new("full-screen", "fas fa-expand-alt"),
        new("windowed-screen", "fas fa-compress-alt"),
        new("chevron-down", "fas fa-chevron-down"),

        // Buttcons and affordances:
        new("close-dialog", "/img/glyphs/xmark-light.svg", "Close", "glyph"),
        new("option-checked", "/img/glyphs/square-check-solid.svg", "Checked", "small-icon"),
        new("option-unchecked", "/img/glyphs/square-regular.svg", "Unchecked", "small-icon"),
        new("option-indeterminate", "/img/glyphs/square-minus-duotone.svg", "Indeterminate", "small-icon"),

        // Header glyphs
        new("modules", "/img/glyphs/grid-solid.svg", "Select Module", "glyph"),
        new("alerts", "/img/glyphs/bell-regular.svg", "Alerts", "glyph"),
        new("newItem", "/img/glyphs/hexagon-plus-regular.svg", "Add Item", "glyph"),

        // Hero icons
        new("assets", "/img/icons/asset-icon.svg", "Assets", "icon"),
        new("logo", "/img/icons/logo.svg", "Site Logo", "icon", SvgRenderType.Reference),
        new("companies-module", "/img/icons/companies-icon.svg", "Company Module", "icon"),
        new("components-module", "/img/icons/components-icon.svg", "Component Module", "icon"),
        new("standard-components", "/img/icons/components-icon-2.svg", "Standard Components", "icon", SvgRenderType.Inline),
        new("dry-components", "/img/icons/components-icon-3.svg", "DRY Components", "icon", SvgRenderType.Inline),
        new("special-components", "/img/icons/components-icon-1.svg", "Special Components", "icon", SvgRenderType.Inline),
        new("api-module", "/img/icons/api-icon.svg", "Integration APIs", "icon"),
        new("overdue", "/img/icons/overdue.svg", "Overdue", "icon", SvgRenderType.Document),

        //Loader icons
        new("loader-error", "fa fa-circle-exclamation"),
        new("loader-timeout", "fa fa-rotate-right"),

    ];


    private Menu CreateMenu() => new() { 
        Icon = "modules",
        Title = "Yellow Jacket",
        Children = [
            new() {
                Icon = "components-module",
                Title = "Blazor Components",
                Subtitle = "Preview and test both standard and DRY components.",
                NavLink = "/components/standard/gravatar",
                ActiveMatch = "/components",
                Children = [
                    new() {
                        Icon = "standard-components",
                        Title = "Standard Components",
                        ActiveMatch = "/components/standard",
                        NavLink = "/components/standard/gravatar",
                        Children = [
                            new() {
                                Icon = "gravatar",
                                Title = "Gravatar",
                                ActiveMatch = "/components/standard/gravatar",
                                NavLink = "/components/standard/gravatar",
                                NavAction = () => navigation.NavigateTo("/components/standard/gravatar")
                            },
                            new() {
                                Icon = "tri-check",
                                Title = "Tri Check",
                                NavLink = "/components/standard/tri-check",
                            },
                            new() {
                                Icon = "tri-switch",
                                Title = "Tri Switch",
                                NavLink = "/components/standard/tri-switch",
                            },
                            new() {
                                Icon = "icons",
                                Title = "Icons",
                                NavLink = "/components/standard/icon",
                            },
                            new() {
                                Icon = "combo-box",
                                Title = "Combo Box",
                                NavLink = "/components/standard/combo-box",
                            },
                            new() {
                                Icon = "mini-card",
                                Title = "Mini Card",
                                NavLink = "/components/standard/mini-card",
                            },
                            new() {
                                Icon = "code-block",
                                Title = "Code Block",
                                NavLink = "/components/standard/code-block",
                            },
                            new() {
                                Icon = "mini-dialog",
                                Title = "Mini Dialog",
                                NavLink = "/components/standard/mini-dialog",
                            },
                            new() {
                                Icon = "flexi-select",
                                Title = "Flexi Select",
                                NavLink = "/components/standard/flexi-select",
                            },
                            new() {
                                Icon = "button",
                                Title = "Button",
                                NavLink = "/components/standard/button",
                            },
                        ],
                    },
                    new() {
                        Icon = "standard-components",
                        Title = "Advanced Components",
                        ActiveMatch = "/components/advanced",
                        NavLink = "/components/advanced/reveal",
                        Children = [
                            new() {
                                Icon = "suspense",
                                Title = "Suspense",
                                ActiveMatch = "/components/advanced/suspense",
                                NavLink = "/components/advanced/suspense",
                                NavAction = () => navigation.NavigateTo("/components/advanced/suspense")
                            },
                            new() {
                                Icon = "reveal",
                                Title = "Reveal",
                                ActiveMatch = "/components/advanced/reveal",
                                NavLink = "/components/advanced/reveal",
                                NavAction = () => navigation.NavigateTo("/components/advanced/reveal")
                            },
                        ],
                    },
                    new() {
                        Icon = "dry-components",
                        Title = "DRY Components",
                        ActiveMatch = "/components/dry",
                        NavLink = "/components/dry/dry-mini-card",
                        Children = [ 
                            new() {
                                Icon = "button",
                                Title = "Button",
                                NavLink = "/components/dry/dry-button",
                            },
                            new() {
                                Icon = "flexi-select",
                                Title = "Flexi Select",
                                NavLink = "/components/dry/dry-flexi-select",
                            },
                            new() {
                                Icon = "mini-card",
                                Title = "Mini Card",
                                NavLink = "/components/dry/dry-mini-card",
                            },
                            new() {
                                Icon = "navigation",
                                Title = "Navigation",
                                NavLink = "/components/dry/dry-navigation",
                            },
                        ],
                    },
                    new() {
                        Icon = "special-components",
                        Title = "Special Components",
                        ActiveMatch = "/components/special",
                        NavLink = "/components/special/theme",
                        Children = [
                            new() {
                                Icon = "error-boundary",
                                Title = "Error Boundary",
                                NavLink = "/components/special/dry-error-boundary",
                            },
                            new() {
                                Icon = "theme",
                                Title = "Theme",
                                NavLink = "/components/special/theme",
                            },
                            new() {
                                Icon = "file-validator",
                                Title = "File Validator",
                                NavLink = "/components/special/file-validator",
                            }
                        ],
                    },
                ],
            },
            new() {
                Icon = "companies-module",
                Title = "Administration",
                ActiveMatch = "/admin",
                NavLink = "/admin/works/companies/list",
                Subtitle = "Administrative functionality for a sample app.  With additional information to break the UI and test ellipsis based word wrapping.",
                Children = [
                    new() {
                        Icon = "assets",
                        Title = "Works",
                        ActiveMatch = "/admin/works",
                        NavLink = "/admin/works/companies/list",
                        Children = [
                            new() {
                                Icon = "contents",
                                Title = "Contents",
                                NavLink = "/admin/works/contents/list",
                            },
                            new() {
                                Icon = "companies",
                                Title = "Companies",
                                ActiveMatch = "/companies",
                                NavLink = "/admin/works/companies/list",
                            },
                            new() {
                                Icon = "employees",
                                Title = "Employees",
                                ActiveMatch = "/employees",
                                NavLink = "/admin/works/employees/list",
                            },
                            new() {
                                Icon = "sectors",
                                Title = "Sectors",
                                NavLink = "/admin/works/sectors/list",
                            },
                            new() {
                                Icon = "regions",
                                Title = "Regions",
                                NavLink = "/admin/works/regions/list",
                            },
                        ],
                    },
                ],
            },
            new() {
                Icon = "api-module",
                Title = "Integration APIs",
                Subtitle = "A set of APIs for programmatic integration with other applications.",
                NavLink = "/swagger",
            },
        ]
    };
}
