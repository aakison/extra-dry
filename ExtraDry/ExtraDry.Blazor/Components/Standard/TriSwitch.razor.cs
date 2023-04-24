namespace ExtraDry.Blazor;

public partial class TriSwitch : ComponentBase, IExtraDryComponent {

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// The value of the checkbox, allowing for indeterminate state.
    /// Use with data binding.
    /// </summary>
    [Parameter]
    public TriSwitchState Value { get; set; } = TriSwitchState.Off;

    /// <summary>
    /// The data-binding event for the `Value`.
    /// </summary>
    [Parameter]
    public EventCallback<TriSwitchState> ValueChanged { get; set; }

    /// <summary>
    /// The Id for the input element of the control. Must be unique.
    /// Defaults to a unique Id.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = $"TriSwitch{++maxId}";

    /// <summary>
    /// The string label that is rendered with the TriSwitch.
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

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    private async Task DoChange(ChangeEventArgs? args)
    {
        if(args == null) {
            args = new ChangeEventArgs();
        }
        else if(args.Value is bool bValue) {
            Value = bValue ? TriSwitchState.On : TriSwitchState.Off;
        }
        args.Value = Value;
        await ValueChanged.InvokeAsync(Value);
        await OnChange.InvokeAsync(args);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(jsIndeterminate != Indeterminate) {
            // Only do interop if we need to change.
            await Module.InvokeVoidAsync("TriSwitch_SetIndeterminate", Id, Indeterminate);
            jsIndeterminate = Indeterminate;
            await DoChange(null);
        }
    }

    private bool Switched => Value == TriSwitchState.On;

    private bool Indeterminate => Value == TriSwitchState.Indeterminate;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "tri-switch", CssClass);

    [Inject]
    private ExtraDryJavascriptModule Module { get; set; } = null!;

    private bool jsIndeterminate = false;

    private static int maxId = 0;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TriSwitchState {
    Off,
    On,
    Indeterminate,
}
