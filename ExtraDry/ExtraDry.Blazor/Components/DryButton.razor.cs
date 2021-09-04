#nullable enable

using ExtraDry.Blazor.Components.Internal;
using ExtraDry.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExtraDry.Blazor {

    /// <summary>
    /// An Extra Dry button executes a command, typically from a ViewModel, with a Model as the argument.
    /// This is typically used from within `DryCommandBar` or `DryForm`, but can be used directly.
    /// If using manually, populate `Command` and `Target`.
    /// </summary>
    public partial class DryButton : ComponentBase {

        /// <summary>
        /// The information, retrieved through reflection, about the method to execute, along with display attributes.
        /// Use `CommandInfo` constructors to create this command if using `DryButton` directly.
        /// </summary>
        [Parameter]
        public CommandInfo? Command { get; set; }

        /// <summary>
        /// The target (method argument) for the command.
        /// If not set, then the target will be inferred from the active `SelectionSet`.
        /// </summary>
        [Parameter]
        public object? Target { get; set; }

        /// <summary>
        /// If both an icon and a caption are available (as defined in the `CommandAttribute` on the method), then display as an Icon only.
        /// </summary>
        [Parameter]
        public bool IconOnly { get; set; }

        [CascadingParameter]
        protected ThemeInfo? ThemeInfo { get; set; }

        [CascadingParameter]
        protected SelectionSet? Selection { get; set; }

        private bool disabled;

        private SelectionSet? selection;

        private bool ShowIcon => !string.IsNullOrWhiteSpace(Command?.Icon);

        private bool ShowCaption => !string.IsNullOrWhiteSpace(Command?.Caption) && !(ShowIcon && IconOnly);

        protected override void OnInitialized()
        {
            if(Target == null) {
                selection = Selection ?? SelectionSet.Lookup(Command?.ViewModel ?? this);
                if(selection != null) {
                    selection.Changed += SelectionChanged;
                    UpdateDisabled();
                }
            }
        }

        private string QualifiedIcon => $"{ThemeInfo?.IconPrefix}{Command?.Icon}";

        private void SelectionChanged(object? sender, SelectionSetChangedEventArgs args)
        {
            UpdateDisabled();
        }

        private void UpdateDisabled()
        {
            if(Command == null) {
                SetEnabled(false);
            }
            else if(selection == null) {
                SetEnabled(true);
            }
            else if(Command.Arguments == CommandArguments.Single) {
                SetEnabled(selection.Single());
            }
            else if(Command.Arguments == CommandArguments.Multiple) {
                SetEnabled(selection.Any());
            }
        }

        private void SetEnabled(bool enabled)
        {
            var newDisabled = !enabled;
            if(disabled != newDisabled) {
                disabled = newDisabled;
                StateHasChanged();
            }
        }

        private async Task Click(MouseEventArgs args)
        {
            if(Command == null) {
                return;
            }
            if(Target != null) {
                await Command.ExecuteAsync(Target);
            }
            else if(Command.Arguments == CommandArguments.Multiple && (selection?.Any() ?? false)) {
                await Command.ExecuteAsync(selection.Items);
            }
            else if(Command.Arguments == CommandArguments.Single && (selection?.Any() ?? false)) {
                await Command.ExecuteAsync(selection.Items.First());
            }
            else if(Command.Arguments == CommandArguments.None) {
                await Command.ExecuteAsync();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if(selection != null) {
                selection.Changed -= SelectionChanged;
            }
        }

    }
}
