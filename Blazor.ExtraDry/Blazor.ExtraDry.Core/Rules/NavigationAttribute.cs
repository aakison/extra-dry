using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class NavigationAttribute : Attribute {

        public NavigationAttribute(string name = "")
        {
            Caption = name;
        }

        public string Group { get; set; } = string.Empty;

        public string Icon { get; set; }

        public string Image { get; set; }

        public string Caption { get; set; }

        public int Order { get; set; }

    }
}
