#nullable enable

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtraDry.Blazor {

    public partial class DryNavigation : ComponentBase, IDisposable {

        [Parameter]
        public object? ViewModel { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? InputAttributes { get; set; }

        [Inject]
        private ILogger<DryNavigation> Logger { get; set; } = null!;

        [Inject]
        private NavigationManager Navigation { get; set; } = null!;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await ScrollNavigation();
        }

        private async void Navigated(object? sender, LocationChangedEventArgs args)
        {
            await ScrollNavigation();
        }

        private async Task ScrollNavigation()
        {
            //await Javascript.InvokeVoidAsync("DryHorizontalScrollNav");
        }

        protected override void OnInitialized()
        {
            Navigation.LocationChanged += Navigated;
        }

        public void Dispose()
        {
            Navigation.LocationChanged -= Navigated;
        }

        private TouchPoint? referencePoint = null;

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
            await ScrollNavigation();
        }

        private const int minimumSwipe = 50;

        [Inject]
        private IJSRuntime Javascript { get; set; } = null!;

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

        protected override void OnParametersSet()
        {
            if(ViewModel == null) {
                Logger.LogError("DryForm requires a ViewModel");
                return;
            }
            if(Description == null) {
                Description = new ViewModelDescription(ViewModel);
            }
        }

        private ViewModelDescription? Description { get; set; }

    }

}
