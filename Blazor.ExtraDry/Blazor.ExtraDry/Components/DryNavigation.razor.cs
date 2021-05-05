#nullable enable

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {
    public partial class DryNavigation : ComponentBase {

        [Parameter]
        public object? ViewModel { get; set; }

        [Inject]
        private ILogger<DryNavigation>? Logger { get; set; }

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
