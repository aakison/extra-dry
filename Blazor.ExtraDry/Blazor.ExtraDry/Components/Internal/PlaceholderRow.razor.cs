using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.ExtraDry.Components.Internal {
    public partial class PlaceholderRow<T> : ComponentBase {

        [Parameter]
        public ViewModelDescription Description { get; set; }

        [Parameter]
        public int Height { get; set; } = 40;

        private Random Random { get; set; } = new Random();
    }
}
