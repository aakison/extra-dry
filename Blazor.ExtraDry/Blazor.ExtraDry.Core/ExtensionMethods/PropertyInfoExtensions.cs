using System.Reflection;

namespace Blazor.ExtraDry.Core.ExtensionMethods {
    public static class PropertyInfoExtensions {
        public static bool IsValueOrImmutable(this PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string);
        }
    }
}
