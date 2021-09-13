#nullable enable

using ExtraDry.Blazor.Components.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace ExtraDry.Blazor.Components.Raw {

    public partial class DrySingleSelect<T> : ComponentBase {

        [Parameter]
        public T? Model { get; set; }

        [Parameter]
        public PropertyDescription? Property { get; set; }

        [Parameter]
        public List<object>? Values { get; set; } 

        [Parameter]
        public EventCallback<ChangeEventArgs>? OnChange { get; set; }

        [Parameter]
        public string? CssClass { get; set; }

        [Inject]
        private ILogger<DryInput<T>> Logger { get; set; } = null!;

        [CascadingParameter]
        public EditMode EditMode { get; set; } = EditMode.Create;

        protected override void OnParametersSet()
        {
            if(Values == null) {
                AllOptions.Clear();
                return;
            }
            if(ValuesEqualOptions()) {
                // Performance: Values unchanged, don't rebuild options
                return;
            }
            int index = 100;
            var selectedObject = Property?.GetValue(Model);
            AllOptions.Clear();
            foreach(var value in Values) {
                var key = index++.ToString();
                var selected = selectedObject?.Equals(value) ?? false;
                var option = new OptionInfo(key, value?.ToString() ?? "-empty-", value) {
                    Selected = selected,
                };
                AllOptions.Add(key, option);
                if(selected) {
                    SelectedOption = option;
                }
            }
            Logger.LogDebug($"DrySingleSelect initialized with {Values?.Count} values");
        }

        private bool ValuesEqualOptions()
        {
            if(Values?.Count != AllOptions.Count) {
                return false;
            }
            var lhs = Values.GetEnumerator();
            var rhs = AllOptions.Values.GetEnumerator();
            while(lhs.MoveNext() && rhs.MoveNext()) {
                if(!lhs.Current.Equals(rhs.Current.Value)) {
                    return false;
                }
            }
            return true;
        }

        private async Task SelectOption(ChangeEventArgs args)
        {
            Logger.LogDebug($"DrySingleSelect Set Option by Key '{args.Value}'");
            var key = args.Value as string;
            if(string.IsNullOrWhiteSpace(key)) {
                return; // selected blank line
            }
            if(SelectedOption != null) {
                SelectedOption.Selected = false;
            }
            var option = AllOptions[key];
            option.Selected = true;
            SelectedOption = option;
            Property?.SetValue(Model, option.Value);
            await InvokeOnChange(args);
        }

        private async Task InvokeOnChange(ChangeEventArgs args)
        {
            var task = OnChange?.InvokeAsync(args);
            if(task != null) {
                await task;
            }
        }

        private bool ReadOnly => EditMode == EditMode.ReadOnly;

        private Dictionary<string, OptionInfo> AllOptions { get; } = new Dictionary<string, OptionInfo>();

        private OptionInfo? SelectedOption { get; set; }

    }

}
