#nullable enable

namespace ExtraDry.Blazor;

public partial class DryMiniDialog : ComponentBase {

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
    /// The caption of the button that is used to clear the title.
    /// Supports HTML markup, do no place customer content in this string.
    /// </summary>
    [Parameter]
    public string ClearButtonCaption { get; set; } = "Clear";

    /// <summary>
    /// Determines if the clear button should be shown.
    /// Default: true
    /// </summary>
    [Parameter]
    public bool ShowClearButton { get; set; } = true;

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
    public string OkButtonCaption { get; set; } = "Close";

    /// <summary>
    /// Determines if the OK/Close button should be shown.
    /// Default: true
    /// </summary>
    [Parameter]
    public bool ShowOkButton { get; set; } = true;

    /// <summary>
    /// If the user clicks outside of the dialog and it loses focus, determines what the dialog should do.
    /// Default: SaveAndClose
    /// </summary>
    [Parameter]
    public MiniDialogAction LoseFocusAction { get; set; } = MiniDialogAction.SaveAndClose;

    /// <summary>
    /// The CSS Class for the root div element of the control.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The child content that renders the form.
    /// Content should be focussed on a single property/field/tuple concept and not attempt to populate an entire object.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback<SelectMiniDialogChangedEventArgs> FilterChanged { get; set; }

    public bool Visible { get; private set; } = true;

    public void Show()
    {
        Visible = true;
        StateHasChanged();
    }

    public void Hide()
    {
        Visible = false;
        StateHasChanged();
    }

    public void Toggle()
    {
        Visible = !Visible;
        StateHasChanged();
    }

    private Task OnFocusIn(FocusEventArgs args)
    {
        shouldCollapse = false;
        return Task.CompletedTask;
    }

    public async Task OnKeyDown(KeyboardEventArgs args)
    {
        await EventsAndRefresh();
    }

    public void OnClearClick(MouseEventArgs _)
    {
        // TODO: Clear Event
    }

    public void OnOkClick(MouseEventArgs _)
    {
        Hide();
    }

    public void OnCancelClick(MouseEventArgs _)
    {
        Hide();
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
                // TODO: Raise save event
            }
            if(LoseFocusAction == MiniDialogAction.Close || LoseFocusAction == MiniDialogAction.SaveAndClose) {
                // TODO: Raise close event
                Hide();
            }
            await EventsAndRefresh();
            shouldCollapse = false;
        }
    }

    private async Task EventsAndRefresh()
    {
        //var args = new SelectMiniDialogChangedEventArgs {
        //    FilterName = Property?.Property?.Name?.ToLowerInvariant() ?? "",
        //    FilterExpression = FilterString,
        //};
    }

    private bool shouldCollapse = false;

}

public enum MiniDialogAction
{
    Save,
    Close,
    SaveAndClose,
    Disabled,
}

public class SelectMiniDialogChangedEventArgs : EventArgs {

    public string FilterName { get; set; } = string.Empty;

    public List<string> FilterValues { get; set; } = new();

    public string FilterExpression { get; set; } = string.Empty;


}
