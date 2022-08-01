#nullable enable

namespace ExtraDry.Blazor;

public partial class TriCheck : ComponentBase {

    public TriCheck()
    {
        Id = $"TriCheck{++maxId}";
    }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public TriCheckState Value { get; set; } = TriCheckState.Unchecked;

    [Parameter]
    public string Id { get; set; }

    public event EventHandler<MouseEventArgs>? OnClicked;

    private void DoClick(MouseEventArgs args)
    {
        OnClicked?.Invoke(this, args);
    }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    private bool Checked {
        get => Value == TriCheckState.Checked;
        set {
            Console.WriteLine($"Checked {value}");
            Value = value ? TriCheckState.Checked : TriCheckState.Unchecked;
        }
    }

    private bool Indeterminate => Value == TriCheckState.Indeterminate;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(jsIndeterminate != Indeterminate) {
            // Only do interop if we need to change.
            await JSRuntime.InvokeVoidAsync("extraDry_setIndeterminate", Id, Indeterminate);
            jsIndeterminate = Indeterminate;
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

