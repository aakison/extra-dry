#nullable enable

using ExtraDry.Blazor.Internal;
using ExtraDry.Blazor.Models;
using ExtraDry.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace ExtraDry.Blazor {

    public partial class DryForm<T> : ComponentBase {
        
        [Parameter]
        public object? ViewModel { get; set; }

        [Parameter]
        public T? Model { get; set; }

        [Parameter]
        public EditMode EditMode { get; set; } = EditMode.Update;

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

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
            if(Model != null) {
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

        private CommandInfo AddNewCommand =>
            new(this, MethodInfoHelper.GetMethodInfo<DryForm<T>>(e => e.AddDefaultElementToList(Array.Empty<int>()))) {
                    Arguments = CommandArguments.Single, Context = CommandContext.Alternate};

        [Command(Name = "Add New", Icon = "plus")]
        private void AddDefaultElementToList(IList items)
        {
            var type = items.GetType().SingleGenericType();
            var instance = type.CreateDefaultInstance();
            items.Add(instance);
            if(Description != null && Model != null) {
                FormDescription = new FormDescription(Description, Model); // re-build description to add/remove UI elements.
            }
            StateHasChanged();
        }

        private void DeleteItem(object? items, object item)
        {
            Console.WriteLine("DeleteItem");
            if(items == null) {
                throw new ArgumentNullException(nameof(items));
            }
            if(items is IList list) {
                Console.WriteLine("  A list");
                list.Remove(item);
                if(Description != null && Model != null) {
                    FormDescription = new FormDescription(Description, Model); // re-build description to add/remove UI elements.
                }
                StateHasChanged();
            }
            else {
                Console.WriteLine("  Not a list: " + items.GetType().ToString());
            }
        }

    }
}
