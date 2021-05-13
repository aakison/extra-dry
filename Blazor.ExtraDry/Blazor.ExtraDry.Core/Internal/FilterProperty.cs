#nullable enable

using System.Reflection;

namespace Blazor.ExtraDry {
    internal class FilterProperty {

        public FilterProperty(PropertyInfo property, FilterAttribute filter)
        {
            Property = property;
            Filter = filter;
        }

        public PropertyInfo Property { get; set; }

        public FilterAttribute Filter { get; set; }

    }
}
