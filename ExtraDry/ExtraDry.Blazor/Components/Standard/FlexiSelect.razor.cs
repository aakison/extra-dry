namespace ExtraDry.Blazor;

/// <summary>
/// A flexi alternative to a select control.  Creates a semantic HTML control with extended 
/// capabilities for generating single and multiple select controls on mobile and desktop 
/// platforms.  Includes list management and filtering.
/// </summary>
/// <typeparam name="TItem">The type for items in the select list.</typeparam>
public partial class FlexiSelect<TItem> : ComponentBase, IExtraDryComponent where TItem : notnull {

    public string Id { get; set; } = $"Id{Guid.NewGuid()}";

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="MiniDialog.Title" />
    [Parameter]
    public string Title { get; set; } = "Select";

    /// <inheritdoc cref="MiniDialog.ShowTitle" />
    [Parameter]
    public bool ShowTitle { get; set; } = true;

    /// <summary>
    /// If any values are selected in the FlexiSelect, then show a preview of those items along 
    /// with an additional count.  Set to false to only display the title regardless of selection.
    /// </summary>
    [Parameter]
    public bool ShowPreview { get; set; } = true;

    /// <inheritdoc cref="Button.Icon" />
    [Parameter]
    public string Icon { get; set; } = string.Empty;

    /// <inheritdoc cref="Button.Affordance" />
    [Parameter]
    public string Affordance { get; set; } = "select";

    /// <inheritdoc cref="IComments.Placeholder"/>
    [Parameter]
    public string Placeholder { get; set; } = "select...";

    /// <inheritdoc cref="FlexiSelectForm{TItem}.ShowFilterThreshold" />
    [Parameter]
    public int ShowFilterThreshold { get; set; } = 10;

    /// <inheritdoc cref="FlexiSelectForm{TItem}.ShowSubtitle" />
    [Parameter]
    public bool? ShowSubtitle { get; set; } = null;

    /// <inheritdoc cref="FlexiSelectForm{TItem}.ShowThumbnail" />
    [Parameter]
    public bool? ShowThumbnail { get; set; }

    /// <inheritdoc cref="FlexiSelectForm{TItem}.FilterPlaceholder" />
    [Parameter]
    public string FilterPlaceholder { get; set; } = "filter";

    /// <inheritdoc cref="FlexiSelectForm{T}.MultiSelect" />
    [Parameter]
    public bool MultiSelect { get; set; }

    /// <summary>
    /// Determines if the dialog should auto-close when in single select mode and a selection has 
    /// been made.  Default true.
    /// </summary>
    [Parameter]
    public bool AutoDismissDialog { get; set; } = true;

    /// <inheritdoc cref="FlexiSelectForm{T}.Data" />
    [Parameter]
    public IEnumerable<TItem>? Data { get; set; }

    /// <inheritdoc cref="MiniDialog.LoseFocusAction" />
    [Parameter]
    public MiniDialogAction LoseFocusAction { get; set; } = MiniDialogAction.SaveAndClose;

    /// <summary>
    /// Event that is fired when the button is clicked and the flexi select has been displayed.  
    /// Will be followed with OnSubmit or OnCancel when user is finished with dialog.
    /// </summary>    
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <inheritdoc cref="MiniDialog.OnSubmit" />
    [Parameter]
    public EventCallback<DialogEventArgs> OnSubmit { get; set; }
  
    /// <inheritdoc cref="MiniDialog.OnCancel" />
    [Parameter]
    public EventCallback<DialogEventArgs> OnCancel { get; set; }

    /// <inheritdoc cref="MiniDialog.AnimationDuration" />
    [Parameter]
    public int AnimationDuration { get; set; } = 100;

    /// <inheritdoc cref="FlexiSelectForm{TItem}.Value" />
    [Parameter]
    public TItem? Value { get; set; }

    /// <inheritdoc cref="FlexiSelectForm{TItem}.ValueChanged" />
    [Parameter]
    public EventCallback<TItem?> ValueChanged { get; set; }

    /// <inheritdoc cref="FlexiSelectForm{TItem}.Values" />
    [Parameter]
    public List<TItem>? Values { get; set; }

    /// <inheritdoc cref="FlexiSelectForm{TItem}.ValuesChanged" />
    [Parameter]
    public EventCallback<List<TItem>?> ValuesChanged { get; set; }

    /// <inheritdoc cref="MiniDialog.DebugCss" />
    [Parameter]
    public bool DebugCss { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    protected async void DoValueChanged(TItem item) {
        await ValueChanged.InvokeAsync(item);
        if(AutoDismissDialog && !MultiSelect && item != null) {
            await MiniDialog!.HideAsync();
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    protected async Task DoClick(MouseEventArgs args)
    {
        await MiniDialog!.ShowAsync();
        await OnClick.InvokeAsync(args);
    }

    protected MiniDialog? MiniDialog { get; set; }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "flexi-select", CssClass);

    private bool DisplayContent => ShowPreview && (Value != null || (Values?.Count ?? 0) != 0);

    private bool DisplayPlaceholder => !DisplayContent;

}
