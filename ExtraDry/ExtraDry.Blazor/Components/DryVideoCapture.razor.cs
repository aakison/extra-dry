namespace ExtraDry.Blazor;

public partial class DryVideoCapture : ComponentBase {

    [Parameter]
    public int Width { get; set; } = 480;

    [Parameter]
    public int Height { get; set; } = 480;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public string Message { get; set; } = "optional photo";

    private FiniteState State { get; set; } = FiniteState.Initializing;

    //private string Pixels(int dimension) => $"{dimension}px";

    // Reference: https://www.meziantou.net/javascript-isolation-in-blazor-components.htm
    private IJSObjectReference? module;

    protected override async Task OnInitializedAsync()
    {
        Console.WriteLine("About to import bundle");
        module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./bundle/extra-dry-blazor-module.min.js");
        State = FiniteState.Waiting;
    }

    private async Task StartVideoCapture()
    {
        if(State == FiniteState.Waiting) {
            if(module != null) {
                await module.InvokeVoidAsync("startCamera");
            }
            State = FiniteState.Video;
            StateHasChanged();
        }
        await Task.Delay(15000);
        if(State == FiniteState.Video) {
            await StopVideoCapture();
            StateHasChanged();
        }
    }

    private async Task CaptureVideo()
    {
        if(module != null) {
            await module.InvokeVoidAsync("captureImage");
        }
    }

    private async Task StopVideoCapture()
    {
        if(module != null) {
            await module.InvokeVoidAsync("stopCamera");
            State = FiniteState.Waiting;
        }
    }

    private enum FiniteState {
        Initializing, Waiting, Video, Preview, BadDevice
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        if(module != null) {
            await module.DisposeAsync();
            module = null;
        }
    }
}
