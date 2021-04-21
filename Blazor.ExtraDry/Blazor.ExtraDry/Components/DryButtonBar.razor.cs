#nullable enable

using Blazor.ExtraDry.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {
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

        private IEnumerable<CommandInfo> SelectCommands(CommandContext context) => Commands.Where(e => e.Context == context);

    }
}
