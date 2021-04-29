#nullable enable

using Blazor.ExtraDry.Internal;
using Blazor.ExtraDry.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {
    public partial class DryForm<T> : ComponentBase {
        
        [Parameter]
        public object? ViewModel { get; set; }

        [Parameter]
        public T? Model { get; set; }

        [Parameter]
        public EditMode EditMode { get; set; } = EditMode.Update;

        [Inject]
        private ILogger<DryForm<T>>? Logger { get; set; }

        private string? error;

        protected override void OnParametersSet()
        {
            if(ViewModel == null) {
                Logger?.LogError("DryForm requires a ViewModel");
                return;
            }
            if(Description == null) {
                Description = new ViewModelDescription(typeof(T), ViewModel);
            }
            if(Model != null && FormDescription == null) {
                FormDescription = new FormDescription(Description, Model);
            }
        }

        private ViewModelDescription? Description { get; set; }

        private FormDescription? FormDescription { get; set; }

        private async Task ExecuteAsync(CommandInfo command)
        {
            try {
                await command.ExecuteAsync(Model);
            }
            catch(DryException ex) {
                error = ex.UserMessage;
            }
            catch(Exception) {
                error = "Unrecognized error occurred";
            }
        }
    }
}
