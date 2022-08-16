#nullable enable

namespace ExtraDry.Blazor;

public partial class TriCheck : ComponentBase, IExtraDryComponent {

    /// <inheritdoc cref="IExtraDryComponent.CssClass" />
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public TriCheckState Value { get; set; } = TriCheckState.Unchecked;

    [Parameter]
    public EventCallback<TriCheckState> ValueChanged { get; set; }

    [Parameter]
    public string Id { get; set; } = $"TriCheck{++maxId}";

    /// <summary>
    /// The string label that is rendered with the TriCheck.
    /// </summary>
    [Parameter]
    public string Label { get; set; } = "label";

    [Parameter]
    public EventCallback<MouseEventArgs> OnClicked { get; set; }

    [Parameter]
    public EventCallback<ChangeEventArgs> OnChange { get; set; }

    /// <inheritdoc cref="IExtraDryComponent.UnmatchedAttributes" />
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> UnmatchedAttributes { get; set; } = null!;

    private async Task DoChange(ChangeEventArgs? args)
    {
        Console.WriteLine("DoChange");
        if(args == null) {
            Console.WriteLine("  args null");
            args = new ChangeEventArgs();
        }
        else if(args.Value is bool bValue) {
            Console.WriteLine($"  args bool value {bValue}");
            Value = bValue ? TriCheckState.Checked : TriCheckState.Unchecked;
        }
        args.Value = Value;
        Console.WriteLine($"  ValueChanged");
        await ValueChanged.InvokeAsync(Value);
        Console.WriteLine($"  OnChange");
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

    private string CssClasses => DataConverter.JoinNonEmpty("tri-check", CssClass);

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
