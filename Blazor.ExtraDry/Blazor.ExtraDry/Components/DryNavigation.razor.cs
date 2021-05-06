#nullable enable

using Blazor.ExtraDry.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {
    public partial class DryNavigation : ComponentBase {

        [Parameter]
        public object? ViewModel { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? InputAttributes { get; set; }

        [Parameter]
        public EventCallback<NavigationChangedEventArgs> OnNavigated { get; set; }

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

        private async Task MenuClicked(NavigationDescription navigation)
        {
            await navigation.ExecuteAsync();
            var args = new NavigationChangedEventArgs(navigation.Caption ?? "");
            await OnNavigated.InvokeAsync(args);
        }

        private ViewModelDescription? Description { get; set; }

    }

    public class NavigationChangedEventArgs {
        public NavigationChangedEventArgs(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}
