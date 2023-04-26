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

    private async Task DoClick(MouseEventArgs args)
    {
        Value = ToggleSwitch;
        await ValueChanged.InvokeAsync(Value);
        await OnClicked.InvokeAsync(args);
    }

    private void OnKeyPressed(KeyboardEventArgs args)
    {
        if(args.Code == "Space") {
            Value = ToggleSwitch;
        }
    }

    private TriSwitchState ToggleSwitch => Value == TriSwitchState.On ? TriSwitchState.Off : TriSwitchState.On;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "tri-switch", CssClass);

    private string CssSwitch => DataConverter.JoinNonEmpty(" ", "switch", Value.ToString().ToLowerInvariant());

}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TriSwitchState {
    Off,
    On,
    Indeterminate,
}
