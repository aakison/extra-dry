using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {
    public class AppViewModel {

        public string Name { get; set; }

        public string Copyright { get; set; }

        public string Version { get; set; }

        public Collection<SectionViewModel> Sections { get; } = new Collection<SectionViewModel>();
    }
}
