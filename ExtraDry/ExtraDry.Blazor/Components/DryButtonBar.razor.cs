#nullable enable

using ExtraDry.Blazor.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;

namespace ExtraDry.Blazor {

    public partial class DryButtonBar : ComponentBase {

        /// <summary>
        /// The information, retrieved through reflection, about the method to execute, along with display attributes.
        /// Use `CommandInfo` constructors to create this command if using `DryButton` directly.
        /// </summary>
        [Parameter]
        public IList<CommandInfo> Commands { get; set; } = new List<CommandInfo>();

        /// <summary>
        /// The target (method argument) for the command.
        /// If not set, then the target will be inferred from the active `SelectionSet`.
        /// </summary>
        [Parameter]
        public object? Target { get; set; }

        [Parameter]
        public object? ViewModel { get; set; }

        /// <summary>
        /// Filter the view model's commands by this category.
        /// </summary>
        [Parameter]
        public string? Category { get; set; }

        private ViewModelDescription? Description { get; set; }

        protected override void OnParametersSet()
        {
            if(!Commands.Any() && ViewModel != null && Description == null) {
                Description = new ViewModelDescription(ViewModel);
                Commands = Description.Commands;
            }
        }

        private IEnumerable<CommandInfo> SelectCommands(CommandContext context) => Commands
            .Where(e => e.Context == context)
            .Where(e => Category == null || Category == e.Category);

    }
}
