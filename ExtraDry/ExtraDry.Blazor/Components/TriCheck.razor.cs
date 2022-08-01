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
        Console.WriteLine($"Checked {Value} to {args?.Value}");
        if(args == null) {
            args = new ChangeEventArgs();
        }
        else if(args.Value is bool bValue) {
            Value = bValue ? TriCheckState.Checked : TriCheckState.Unchecked;
        }
        args.Value = Value;
        await OnChange.InvokeAsync(args);
    }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    private bool Checked => Value == TriCheckState.Checked;

    private bool Indeterminate => Value == TriCheckState.Indeterminate;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(jsIndeterminate != Indeterminate) {
            // Only do interop if we need to change.
            await JSRuntime.InvokeVoidAsync("extraDry_setIndeterminate", Id, Indeterminate);
            jsIndeterminate = Indeterminate;
            await DoChange(null);
        }
    }

    private bool jsIndeterminate = false;

    private static int maxId = 0;
}

public enum TriCheckState
{
    Unchecked,
    Checked,
    Indeterminate,
}

