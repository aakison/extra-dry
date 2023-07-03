namespace ExtraDry.Blazor;

public partial class TriCheck : ComponentBase, IExtraDryComponent {

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The value of the checkbox, allowing for indeterminate state.
    /// Use with data binding.
    /// </summary>
    [Parameter]
    public TriCheckState Value { get; set; } = TriCheckState.Unchecked;

    /// <summary>
    /// The data-binding event for the `Value`.
    /// </summary>
    [Parameter]
    public EventCallback<TriCheckState> ValueChanged { get; set; }

    /// <summary>
    /// The Id for the input element of the control. Must be unique.
    /// Defaults to a unique Id.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = $"TriCheck{++maxId}";

    /// <summary>
    /// The string label that is rendered with the TriCheck.
    /// </summary>
    [Parameter]
    public string Label { get; set; } = "label";

    /// <summary>
    /// Event callback for clicking on the input, fired on user-input.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClicked { get; set; }

    /// <summary>
    /// Event callback for change of input.
    /// </summary>
    [Parameter]
    public EventCallback<ChangeEventArgs> OnChange { get; set; }

    /// <summary>
    /// Indicates if icons are used in addition to the rendered checkbox.
    /// </summary>
    [Parameter]
    public bool UseIcons { get; set; }

    /// <summary>
    /// If `UseIcons` is true, the icon to display when the item is checked.  This is a key from
    /// the Icons and defaults to "option-checked".
    /// </summary>
    [Parameter]
    public string CheckedIcon { get; set; } = "option-checked";

    /// <summary>
    /// If `UseIcons` is true, the icon to display when the item is not checked.  This is a key from
    /// the Icons and defaults to "option-unchecked".
    /// </summary>
    [Parameter]
    public string UncheckedIcon { get; set; } = "option-unchecked";

    /// <summary>
    /// If `UseIcons` is true, the icon to display when the item is indeterminate.  This is a key from
    /// the Icons and defaults to "option-indeterminate".
    /// </summary>
    [Parameter]
    public string IndeterminateIcon { get; set; } = "option-indeterminate";

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    private async Task DoChange(ChangeEventArgs? args)
    {
        if(args == null) {
            args = new ChangeEventArgs();
        }
        else if(args.Value is bool bValue) {
            Value = bValue ? TriCheckState.Checked : TriCheckState.Unchecked;
        }
        args.Value = Value;
        await ValueChanged.InvokeAsync(Value);
        await OnChange.InvokeAsync(args);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(jsIndeterminate != Indeterminate) {
            // Only do interop if we need to change.
            await Module.InvokeVoidAsync("TriCheck_SetIndeterminate", Id, Indeterminate);
            jsIndeterminate = Indeterminate;
            await DoChange(null);
        }
    }

    private bool Checked => Value == TriCheckState.Checked;

    private bool Indeterminate => Value == TriCheckState.Indeterminate;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "tri-check", CssClass);

    private string CurrentIcon => Value switch {
            TriCheckState.Checked => CheckedIcon,
            TriCheckState.Unchecked => UncheckedIcon,
            _ => IndeterminateIcon,
        };

    [Inject]
    private ExtraDryJavascriptModule Module { get; set; } = null!;

    private bool jsIndeterminate = false;

    private static int maxId = 0;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TriCheckState
{
    Unchecked,
    Checked,
    Indeterminate,
}
