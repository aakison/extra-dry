using ExtraDry.Core;
using System.Reflection;

namespace ExtraDry.Server.Internal {

    /// <summary>
    /// Encapsulates a property that has the `FilterAttribute` on it.
    /// </summary>
    internal class FilterProperty {

        public FilterProperty(PropertyInfo property, FilterAttribute filter, string? externalName = null)
        {
            Property = property;
            Filter = filter;
            ExternalName = externalName ?? property.Name;
        }

        public PropertyInfo Property { get; set; }

        public FilterAttribute Filter { get; set; }

        public string ExternalName { get; }

    }
}
