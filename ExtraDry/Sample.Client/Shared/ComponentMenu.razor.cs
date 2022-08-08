#nullable enable

using ExtraDry.Core;
using Microsoft.AspNetCore.Components;

namespace Sample.Client.Shared {

    public sealed partial class ComponentMenu : ComponentBase {


        [Inject]
        private NavigationManager Navigation { get; set; } = null!;


        [Navigation(Icon = "register")]
        public string Register => $"/dummy/a";

        [Navigation(Icon = "contents")]
        public string Contents => $"/contents";

        [Navigation(Icon = "companies")]
        public string Companies => $"/companies";

        [Navigation(Icon = "sectors")]
        public string Sectors => $"/sectors";

        [Navigation(Icon = "dummy")]
        public string Dummy3 => $"/dummy/3";

        [Navigation(Icon = "dummy")]
        public string Dummy4 => $"/dummy/4";

        [Navigation(Icon = "dummy")]
        public string Dummy5 => $"/dummy/5";

    }
}
