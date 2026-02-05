using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ExtraDry.Blazor;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "DRY1500:Extra DRY Blazor components should have an interface.", Justification = "Pending decision on fate of component.")]
public partial class DryButtonBar(
    AuthenticationStateProvider AuthProvider)
    : ComponentBase, IExtraDryComponent
{
    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The information, retrieved through reflection, about the method to execute, along with
    /// display attributes. Use `CommandInfo` constructors to create this command if using
    /// `DryButton` directly.
    /// </summary>
    [Parameter]
    public IList<CommandInfo> Commands { get; set; } = [];

    /// <summary>
    /// The target (method argument) for the command. If not set, then the target will be inferred
    /// from the active `SelectionSet`.
    /// </summary>
    [Parameter]
    public object? Model { get; set; }

    /// <summary>
    /// A link to the Decorator that contains the method to call on the button click. Typically
    /// `this` when View is doubling as Decorator. 
    /// </summary>
    [Parameter, EditorRequired]
    public object Decorator { get; set; } = null!;

    /// <summary>
    /// Filter the view model's commands by this category.
    /// </summary>
    [Parameter]
    public string? Category { get; set; }

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    private DecoratorInfo? Description { get; set; }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "buttons", CssClass);

    protected override async Task OnParametersSetAsync()
    {
        if(!Commands.Any() && Decorator != null && Description == null) {
            Description = new DecoratorInfo(Decorator);
            Commands = Description.Commands;
        }
        var provider = await AuthProvider.GetAuthenticationStateAsync();
        var user = provider.User;
        AuthorizedCommands = Commands.Where(e => user.IsInAnyRole(e.Roles));
    }

    private IEnumerable<CommandInfo> AuthorizedCommands { get; set; } = [];

    private IEnumerable<CommandInfo> SelectCommands(CommandContext context)
    {
        return AuthorizedCommands
            .Where(e => e.Context == context)
            .Where(e => Category == null || Category == e.Category);
    }
}
