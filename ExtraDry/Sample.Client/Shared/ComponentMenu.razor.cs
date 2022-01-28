#nullable enable

using ExtraDry.Blazor;
using ExtraDry.Core;
using Microsoft.AspNetCore.Components;

namespace Sample.Client.Shared {

    public sealed partial class ComponentMenu : ComponentBase {


        [Inject]
        private NavigationManager Navigation { get; set; } = null!;


        [Navigation(Icon = "home")]
        public string Register => $"/dummy/a";

        [Navigation(Icon = "briefcase")]
        public string Locations => $"/dummy/0";

        [Navigation(Icon = "briefcase")]
        public string Dummy1 => $"/dummy/1";

        [Navigation(Icon = "briefcase")]
        public string Dummy2 => $"/dummy/2";

        [Navigation(Icon = "briefcase")]
        public string Dummy3 => $"/dummy/3";

        [Navigation(Icon = "briefcase")]
        public string Dummy4 => $"/dummy/4";

        [Navigation(Icon = "briefcase")]
        public string Dummy5 => $"/dummy/5";

    }
}
