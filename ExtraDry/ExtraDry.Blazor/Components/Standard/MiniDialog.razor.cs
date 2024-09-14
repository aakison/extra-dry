namespace ExtraDry.Blazor;

/// <summary>
/// A mini-dialog box that can be used to edit a single property or field.  Is typically shown as a
/// dropdown from a button or link.  On small screens (i.e. phones), it may be displayed full screen
/// to support all the options of just a single field.  The dialog box is shown and hidden using the 
/// Show() and Hide() methods.
/// </summary>
public partial class MiniDialog : ComponentBase, IExtraDryComponent, IDisposable
{

    /// <summary>
    /// The title for the dialog box.
    /// </summary>
    [Parameter]
    public string Title { get; set; } = "Title";

    /// <summary>
    /// Determines if the title should be shown.
    /// Default: true
    /// </summary>
    [Parameter]
    public bool ShowTitle { get; set; } = true;

    /// <summary>
    /// The caption of the button that is used to cancel the dialog box.
    /// Supports HTML markup, do no place customer content in this string.
    /// </summary>
    [Parameter]
    public string CancelButtonCaption { get; set; } = "&times;";

    /// <summary>
    /// Determines if the cancel button should be shown.
    /// Default: true
    /// </summary>
    [Parameter]
    public bool ShowCancelButton { get; set; } = true;

    /// <summary>
    /// The caption of the button that is used to save and close the dialog box.
    /// Supports HTML markup, do no place customer content in this string.
    /// </summary>
    [Parameter]
    public string SubmitButtonCaption { get; set; } = "Close";

    /// <summary>
    /// Determines if the OK/Close button should be shown.
    /// Default: true
    /// </summary>
    [Parameter]
    public bool ShowSubmitButton { get; set; } = true;

    /// <inheritdoc cref="MiniDialogAction"/>
    [Parameter]
    public MiniDialogAction LoseFocusAction { get; set; } = MiniDialogAction.SaveAndClose;

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The child content that renders the form.
    /// Content should be focussed on a single property/field/tuple concept and not attempt to populate an entire object.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Called when the mini-dialog is closed using the Submit button, 
    /// or when it loses focus and the LoseFocusAction is Save or SaveAndClose.
    /// </summary>
    [Parameter]
    public EventCallback<DialogEventArgs> OnSubmit { get; set; }

    /// <summary>
    /// Called when the mini-dialog is closed using the cancel button,
    /// or when it loses focus and the LoseFocusAction is Cancel.
    /// </summary>
    [Parameter]
    public EventCallback<DialogEventArgs> OnCancel { get; set; }

    /// <summary>
    /// The number of milliseconds to render the showing and hiding states.
    /// This only affects the duration of the CSS classes applied to the dialog.
    /// The user must use CSS to have these style perform the desired animations.
    /// </summary>
    [Parameter]
    public int AnimationDuration { get; set; }

    /// <inheritdoc cref="ComboBox{TItem}.DebugCss" />
    [Parameter]
    public bool DebugCss { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    /// <summary>
    /// The state of the dialog box, which cycles when Show() and Hide() are called.
    /// On Show, typically moves: NotLoaded -> Hidden -> Showing -> Visible.
    /// On Hide, typically moves: Visible -> Hiding -> Hidden -> NotLoaded.
    /// The states are also reflected in CSS classes that are rendered except for NotLoaded as no HTML is rendered.
    /// Also, Show and Hide can interrupt each other so the actual flow might bounce around.
    /// </summary>
    public DialogState State { get; private set; } = DialogState.NotLoaded;

    /// <summary>
    /// Show the mini dialog box, cylcing through CSS states for animation.
    /// </summary>
    public async Task ShowAsync()
    {
        CancelAndResetCancellation();
        // No matter what, to show we need to first make sure it's loaded and rendered to DOM.
        if(await ChangeStateAsync(DialogState.NotLoaded, DialogState.Hidden, minimumDuration)) {
            return;
        }
        // Start the animation from hidden state to showing state.
        if(await ChangeStateAsync(DialogState.Hidden, DialogState.Showing, ActualAnimationDuration)) {
            return;
        }
        // If we're in a hiding state, then must have interupted the Hide to re-show, get on it...
        if(await ChangeStateAsync(DialogState.Hiding, DialogState.Showing, ActualAnimationDuration)) {
            return;
        }
        // Finish animation, if any, and rest on visible state.
        if(await ChangeStateAsync(DialogState.Showing, DialogState.Visible, minimumDuration)) {
            return;
        }
        await Form.FocusAsync(); // Get focus into visible control so OnFocusOut can fire.
        StateHasChanged();
    }

    /// <summary>
    /// Hide the mini dialog box, cycling through CSS states for animation.
    /// </summary>
    public async Task HideAsync()
    {
        CancelAndResetCancellation();
        // When visible, just start hiding.
        if(await ChangeStateAsync(DialogState.Visible, DialogState.Hiding, ActualAnimationDuration)) {
            return;
        }
        // If we're in a showing state, must have been interrupted, get to hiding...
        if(await ChangeStateAsync(DialogState.Showing, DialogState.Hiding, ActualAnimationDuration)) {
            return;
        }
        // Finish animation, if any, then finish into final animation state.
        if(await ChangeStateAsync(DialogState.Hiding, DialogState.Hidden, minimumDuration)) {
            return;
        }
        // Finally, unload the dialog box to conserve resources.
        if(await ChangeStateAsync(DialogState.Hidden, DialogState.NotLoaded, minimumDuration)) {
            return;
        }
        StateHasChanged();
    }

    /// <summary>
    /// Show/Hide the mini dialog box depending on the current state, cycling through CSS states 
    /// for animation.
    /// </summary>
    public async Task ToggleAsync()
    {
        if(State == DialogState.NotLoaded || State == DialogState.Hidden || State == DialogState.Hiding) {
            await ShowAsync();
        }
        else {
            await HideAsync();
        }
    }

    /// <summary>
    /// Dispose of managed resources.
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if(stateChangeCancellation != null) {
            stateChangeCancellation.Dispose();
            stateChangeCancellation = null!;
        }
    }

    protected ElementReference Form { get; set; }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "mini-dialog", StateClass, CssClass);

    private async Task<bool> ChangeStateAsync(DialogState from, DialogState to, int duration)
    {
        if(State == from) {
            // UI will load content at this point.
            State = to;
            StateHasChanged();
            try {
                await Task.Delay(duration, stateChangeCancellation.Token);
            }
            catch(TaskCanceledException) {
                // Ignore exception as expected result is return value.
            }
            return stateChangeCancellation.IsCancellationRequested;
        }
        return false;
    }

    private void CancelAndResetCancellation()
    {
        stateChangeCancellation.Cancel();
        stateChangeCancellation = new CancellationTokenSource();
    }

    private Task OnFocusIn(FocusEventArgs args)
    {
        shouldCollapse = false;
        return Task.CompletedTask;
    }

    private async Task OnKeyDown(KeyboardEventArgs args)
    {
        if(args.Code == "Escape") {
            await DoCancel(null);
        }
        if(args.Code == "Enter") {
            await DoSubmit(null);
        }
    }

    private async Task DoSubmit(MouseEventArgs? args)
    {
        var eventArgs = new DialogEventArgs {
            MouseEventArgs = args,
        };
        await OnSubmit.InvokeAsync(eventArgs);
        if(!eventArgs.Cancel) {
            await HideAsync();
        }
    }

    private async Task DoCancel(MouseEventArgs? args)
    {
        var eventArgs = new DialogEventArgs {
            MouseEventArgs = args,
        };
        await OnCancel.InvokeAsync(eventArgs);
        if(!eventArgs.Cancel) {
            await HideAsync();
        }
    }

    private async Task OnFocusOut(FocusEventArgs args)
    {
        if(LoseFocusAction == MiniDialogAction.Disabled) {
            return;
        }
        shouldCollapse = true;
        // wait and see if we should ignore the out because we're switching focus within control.
        await Task.Delay(1);
        if(shouldCollapse) {
            if(LoseFocusAction == MiniDialogAction.Save || LoseFocusAction == MiniDialogAction.SaveAndClose) {
                await DoSubmit(null);
            }
            if(LoseFocusAction == MiniDialogAction.Cancel) {
                await DoCancel(null);
            }
            shouldCollapse = false;
        }
    }

    private string StateClass => State.ToString().ToLowerInvariant();

    private bool Visible => State != DialogState.NotLoaded;

    private CancellationTokenSource stateChangeCancellation = new();

    private bool shouldCollapse;

    private int ActualAnimationDuration => Math.Clamp(AnimationDuration, 0, maximumDuration);

    // One frame to allow refresh to happen.
    private const int minimumDuration = 1000 / 60;

    // Maximum to some logical upper bound that prevents app-destructive behavior.
    private const int maximumDuration = 5000;

}
