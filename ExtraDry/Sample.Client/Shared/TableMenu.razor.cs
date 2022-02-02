#nullable enable

using ExtraDry.Blazor;
using Microsoft.AspNetCore.Components;

namespace Sample.Client.Shared {

    public sealed partial class TableMenu : ComponentBase {


        [Inject]
        private NavigationManager Navigation { get; set; } = null!;


        [Command(Icon = "plus")]
        public void AddItem() { }

        [Command(Icon = "filter")]
        public void Filter() { }

        [Command(Icon = "expand-alt")]
        public void Expand() { }

    }
}
