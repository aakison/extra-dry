using Microsoft.AspNetCore.Components.Routing;

namespace ExtraDry.Blazor;

public partial class DryNavigation : ComponentBase, IDisposable {

    [Parameter, EditorRequired]
    public object ViewModel { get; set; } = null!;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? InputAttributes { get; set; }

    public void NavigateToPrevious()
    {
        if(Description == null || Navigation == null) {
            return;
        }
        var previous = Description.Navigations.Reverse()
            .SkipWhile(e => !e.UriMatch(Navigation))
            .SkipWhile(e => e.UriMatch(Navigation))
            .FirstOrDefault() ?? Description.Navigations.FirstOrDefault();
        previous?.Navigate(Navigation);
    }

    public void NavigateToNext()
    {
        if(Description == null || Navigation == null) {
            return;
        }
        var next = Description.Navigations
            .SkipWhile(e => !e.UriMatch(Navigation))
            .SkipWhile(e => e.UriMatch(Navigation))
            .FirstOrDefault() ?? Description.Navigations.LastOrDefault();
        next?.Navigate(Navigation);
    }

    public void Dispose()
    {
        Navigation.LocationChanged -= Navigated;
    }

    protected override void OnParametersSet()
    {
        if(ViewModel == null) {
            Logger.LogError("DryForm requires a ViewModel");
            return;
        }
        if(Description == null || Description.ViewModel != ViewModel) {
            Description = new ViewModelDescription(ViewModel);
        }
    }

    protected override void OnInitialized()
    {
        Navigation.LocationChanged += Navigated;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await ScrollNavigation();
    }

    private void Navigated(object? sender, LocationChangedEventArgs args)
    {
        StateHasChanged();
    }

    private async Task ScrollNavigation()
    {
        await Module.InvokeVoidAsync("DryNavigation_HorizontalScrollNav");
    }

    private void HandleTouchStart(TouchEventArgs t)
    {
        referencePoint = t.TargetTouches[0];
    }

    private async void HandleTouchEnd(TouchEventArgs args)
    {
        var finishPoint = args.ChangedTouches[0];
        if(referencePoint != null && finishPoint.Identifier == referencePoint.Identifier) {
            var deltaX = finishPoint.ScreenX - referencePoint.ScreenX;
            if(deltaX > minimumSwipe) {
                Console.WriteLine("Swipe Right");
                NavigateToPrevious();
            }
            else if(deltaX < -minimumSwipe) {
                Console.WriteLine("Swipe Left");
                NavigateToNext();
            }
        }
        await Task.Delay(16); // KLUDGE: Wait a frame for the navigation elements to update before calling to JS to animate.
        Console.WriteLine("Scroll TouchEnd");
        await ScrollNavigation();
    }

    private const int minimumSwipe = 50;

    private TouchPoint? referencePoint = null;

    [Inject]
    private ILogger<DryNavigation> Logger { get; set; } = null!;

    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    [Inject]
    private ExtraDryJavascriptModule Module { get; set; } = null!;

    private ViewModelDescription? Description { get; set; }

}
