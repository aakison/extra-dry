#nullable enable

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.ExtraDry {
    public partial class DryNavigation : ComponentBase {

        [Parameter]
        public object? ViewModel { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? InputAttributes { get; set; }

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

        [Inject]
        private ILogger<DryNavigation>? Logger { get; set; }

        [Inject]
        private NavigationManager? Navigation { get; set; }
        
        protected override void OnParametersSet()
        {
            if(ViewModel == null) {
                Logger?.LogError("DryForm requires a ViewModel");
                return;
            }
            if(Description == null) {
                Description = new ViewModelDescription(ViewModel);
            }
        }

        private ViewModelDescription? Description { get; set; }

    }

}
