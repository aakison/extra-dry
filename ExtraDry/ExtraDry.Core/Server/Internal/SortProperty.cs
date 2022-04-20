using System.Linq;
using System.Reflection;

namespace ExtraDry.Server.Internal {

    internal class SortProperty {
        public SortProperty(PropertyInfo property, string externalName)
        {
            Property = property;
            ExternalName = externalName;
        }

        public PropertyInfo Property { get; }
        public string ExternalName { get; }
    }
}
