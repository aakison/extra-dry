namespace ExtraDry.Blazor;

public partial class DryFlexiSelect : DryPropertyComponentBase, IExtraDryComponent {

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <inheritdoc cref="FlexiSelect{TItem}.Title" />
    [Parameter]
    public string Title { get; set; } = "Select";

    /// <inheritdoc cref="FlexiSelect{TItem}.ShowTitle" />
    [Parameter]
    public bool ShowTitle { get; set; } = true;

    /// <inheritdoc cref="FlexiSelect{TItem}.Placeholder"/>
    [Parameter]
    public string Placeholder { get; set; } = "select...";

    /// <inheritdoc cref="FlexiSelect{TItem}.ShowFilterThreshold" />
    [Parameter]
    public int ShowFilterThreshold { get; set; } = 10;

    /// <inheritdoc cref="FlexiSelect{TItem}.FilterPlaceholder" />
    [Parameter]
    public string FilterPlaceholder { get; set; } = "filter";

    /// <inheritdoc cref="FlexiSelect{TItem}.AutoDismissDialog" />
    [Parameter]
    public bool AutoDismissDialog { get; set; } = true;

    /// <inheritdoc cref="MiniDialog.LoseFocusAction" />
    [Parameter]
    public MiniDialogAction LoseFocusAction { get; set; } = MiniDialogAction.SaveAndClose;

    /// <inheritdoc cref="FlexiSelect{TItem}.OnClick"/>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <inheritdoc cref="FlexiSelect{TItem}.OnSubmit" />
    [Parameter]
    public EventCallback<DialogEventArgs> OnSubmit { get; set; }

    /// <inheritdoc cref="FlexiSelect{TItem}.OnCancel" />
    [Parameter]
    public EventCallback<DialogEventArgs> OnCancel { get; set; }

    /// <inheritdoc cref="FlexiSelect{TItem}.AnimationDuration" />
    [Parameter]
    public int AnimationDuration { get; set; } = 100;

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? UnmatchedAttributes { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        Data = Property?.GetDiscreteValues();
    }

    protected IList<ValueDescription>? Data { get; set; }

    private ValueDescription? LocalValue {
        get => localValue;
        set {
            this.localValue = value;
            Property?.SetValue(Model, this.localValue?.Key);
        }
    }
    private ValueDescription? localValue;

}
