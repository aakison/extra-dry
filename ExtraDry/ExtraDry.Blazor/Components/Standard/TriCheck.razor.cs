#nullable enable

namespace ExtraDry.Blazor;

public partial class TriCheck : ComponentBase {

    public TriCheck()
    {
        Id = $"TriCheck{++maxId}";
        Label = "label";
    }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public TriCheckState Value { get; set; } = TriCheckState.Unchecked;

    [Parameter]
    public string Id { get; set; }

    [Parameter]
    public string Label { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClicked { get; set; }

    [Parameter]
    public EventCallback<ChangeEventArgs> OnChange { get; set; }

    private async Task DoChange(ChangeEventArgs? args)
    {
        if(args == null) {
            args = new ChangeEventArgs();
        }
        else if(args.Value is bool bValue) {
            Value = bValue ? TriCheckState.Checked : TriCheckState.Unchecked;
        }
        args.Value = Value;
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

