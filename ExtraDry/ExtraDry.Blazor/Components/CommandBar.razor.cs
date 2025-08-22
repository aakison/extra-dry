using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraDry.Blazor;

public partial class CommandBar(
    AuthenticationStateProvider AuthProvider) 
    : ComponentBase, IExtraDryComponent
{
    [Parameter, EditorRequired]
    public object Decorator { get; set; } = null!;

    [Parameter]
    public string CssClass { get; set; } = "";

    [Parameter]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; } = [];


    protected override async Task OnParametersSetAsync()
    {
        Description ??= new DecoratorInfo(Decorator);
        var provider = await AuthProvider.GetAuthenticationStateAsync();
        var user = provider.User;
        AuthorizedMenuCommands = Description.MenuCommands.Where(e => user.IsInAnyRole(e.Roles));
        AuthorizedMultiContextCommands = Description.MultiContextCommands.Where(e => user.IsInAnyRole(e.Roles));
        AuthorizedContextCommands = Description.ContextCommands.Where(e => user.IsInAnyRole(e.Roles));
    }

    private DecoratorInfo? Description { get; set; }

    private IEnumerable<CommandInfo> AuthorizedMenuCommands { get; set; } = [];

    private IEnumerable<CommandInfo> AuthorizedMultiContextCommands { get; set; } = [];

    private IEnumerable<CommandInfo> AuthorizedContextCommands { get; set; } = [];
}
