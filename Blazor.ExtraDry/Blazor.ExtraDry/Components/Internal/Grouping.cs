using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.ExtraDry.Components.Internal {
    public class Grouping {

        public string GroupingColumn { get; set; }

        public int GroupingDepth { get; set; }

        public bool IsGroup { get; set; }

        public bool IsExpanded { get; set; }

        public Action Toggle { get; set; }

    }
}
