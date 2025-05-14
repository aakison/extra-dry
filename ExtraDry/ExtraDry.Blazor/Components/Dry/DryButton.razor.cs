using ExtraDry.Blazor.Components.Internal;

namespace ExtraDry.Blazor;

/// <summary>
/// An Extra Dry button executes a command, typically from a ViewModel, with a Model as the
/// argument. This is typically used from within `DryCommandBar` or `DryForm`, but can be used
/// directly. If using manually, populate ViewModel and MethodName, and optionally Model.
/// </summary>
public partial class DryButton : ComponentBase, IExtraDryComponent
{
    /// <inheritdoc />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// A link to the ViewModel that contains the method to call on the button click. Typically
    /// `this`. When used, must also be partnered with `MethodName` being set.
    /// </summary>
    [Parameter]
    public object? ViewModel { get; set; }

    /// <summary>
    /// When defining the method using the ViewModel parameter, the method name to call. The method
    /// should either take no parameters, a single Model or an enumeration of Models.
    /// </summary>
    [Parameter]
    public string? MethodName { get; set; }

    /// <summary>
    /// A CommandInfo object that defines the view model and method. This parameter is mutually
    /// exclusive from the ViewModel/MethodName. Use CommandInfo constructors to create this
    /// command if using DryButton directly, typically this parameter is only used if the caller
    /// already has the CommandInfo from a ViewModelDescription.
    /// </summary>
    [Parameter]
    public CommandInfo? Command { get; set; }

    /// <summary>
    /// The optional argument for the command if the command takes one. Typically the model or
    /// models for the command are determined based on the context of the page and this is not set
    /// directly. When unset, the `SelectionSet` active for the Decorator will have one or more
    /// models that will determine the arguments for the method.
    /// </summary>
    [Parameter]
    public object? Model { get; set; }

    /// <summary>
    /// If both an icon and a caption are available (as defined in the `CommandAttribute` on the
    /// method), then display as an Icon only.
    /// </summary>
    [Parameter]
    public bool IconOnly { get; set; }

    /// <summary>
    /// Set to enable/disable the button. Set to `null` (the default) to use the existence of
    /// Models in the `SelectionSet` to determine if the button should be enabled.
    /// </summary>
    [Parameter]
    public bool? Enabled { get; set; }

    /// <inheritdoc />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        if(SelectionAccessor != null) {
            SelectionAccessor.SelectionSet.Changed -= SelectionChanged;
        }
    }

    private SelectionSetAccessor? SelectionAccessor { get; set; }

    [Inject]
    protected ILogger<DryButton> Logger { get; set; } = null!;

    protected override void OnParametersSet()
    {
        if(Command is not null) {
            if(ViewModel is not null || MethodName is not null) {
                Logger.LogConsoleWarning("Command should be used mutually exclusively with ViewModel and MethodName in DryButton.  The Command property takes precedence and the ViewModel and MethodName will be ignored.");
            }
            ResolvedCommand = Command;
        }
        else if(ViewModel is not null || MethodName is not null) {
            if(ViewModel is null || MethodName is null) {
                Logger.LogConsoleError("When using ViewModel and MethodName to define a command, both must be set.  Button command can not be resolved.");
            }
            else {
                ResolvedCommand = new CommandInfo(ViewModel, MethodName);
            }
        }
        else {
            Logger.LogConsoleError("DryButton must define the command by either providing the Command parameter or both the ViewModel and the MethodName parameters.");
        }

        if(Model is null && ResolvedCommand != null) {
            if(SelectionAccessor is null) {
                SelectionAccessor = new SelectionSetAccessor(ResolvedCommand.ViewModel);
                SelectionAccessor.SelectionSet.Changed += SelectionChanged;
            }
        }

        UpdateDisabled();
    }

    /// <summary>
    /// The information, retrieved through reflection, about the method to execute, along with
    /// display attributes.
    /// </summary>
    //[Parameter]
    private CommandInfo? ResolvedCommand { get; set; }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", CssClass, ResolvedCommand?.DisplayClass);

    private bool HasIcon => !string.IsNullOrWhiteSpace(ResolvedCommand?.Icon);

    private bool ShowCaption => !string.IsNullOrWhiteSpace(ResolvedCommand?.Caption) && !(HasIcon && IconOnly);

    private string ButtonCaption => ShowCaption ? ResolvedCommand?.Caption! : string.Empty;

    private void SelectionChanged(object? sender, SelectionSetChangedEventArgs args)
    {
        UpdateDisabled();
    }

    private void UpdateDisabled()
    {
        if(ResolvedCommand is null) {
            SetEnabled(false);
        }
        else if(SelectionAccessor is null) {
            SetEnabled(true);
        }
        else if(ResolvedCommand.Arguments == CommandArguments.Single) {
            if(Model is null) {
                SetEnabled(SelectionAccessor.SelectionSet.Single());
            }
            else {
                SetEnabled(true);
            }
        }
        else if(ResolvedCommand.Arguments == CommandArguments.Multiple) {
            SetEnabled(SelectionAccessor.SelectionSet.Any());
        }
    }

    private void SetEnabled(bool enabled)
    {
        var newDisabled = !enabled;
        if(disabled != newDisabled) {
            disabled = newDisabled;
            StateHasChanged();
        }
    }

    private bool ResolvedEnabled => Enabled ?? !disabled;

    private async Task DoClick(MouseEventArgs _)
    {
        if(ResolvedCommand is null) {
            return;
        }
        if(Model != null) {
            await ResolvedCommand.ExecuteAsync(Model);
        }
        else if(ResolvedCommand.Arguments == CommandArguments.Multiple && (SelectionAccessor?.SelectionSet.Any() ?? false)) {
            await ResolvedCommand.ExecuteAsync(SelectionAccessor.SelectionSet.Items);
        }
        else if(ResolvedCommand.Arguments == CommandArguments.Single && (SelectionAccessor?.SelectionSet.Any() ?? false)) {
            await ResolvedCommand.ExecuteAsync(SelectionAccessor.SelectionSet.Items.First());
        }
        else if(ResolvedCommand.Arguments == CommandArguments.None) {
            Console.WriteLine("No Args");
            await ResolvedCommand.ExecuteAsync();
        }
    }

    private bool disabled;
}
