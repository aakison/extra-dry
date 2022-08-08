using System;

namespace ExtraDry.Core {

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class NavigationAttribute : Attribute {

        public NavigationAttribute(string name = "")
        {
            Caption = name;
        }

        public string Group { get; set; } = string.Empty;

        /// <summary>
        /// The optional icon to render when creating the navigation link.
        /// Icon details are looked up in the `Theme` Icons collection.
        /// </summary>
        public string? Icon { get; set; }

        public string? Image { get; set; }

        public string Caption { get; set; }

        public int Order { get; set; }

        public string? ActiveMatch { get; set; }

    }
}
