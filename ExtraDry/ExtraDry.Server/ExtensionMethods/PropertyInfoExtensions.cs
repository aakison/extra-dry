using System.Reflection;

namespace ExtraDry.Server;

public static class PropertyInfoExtensions {
    public static bool IsValueOrImmutable(this PropertyInfo propertyInfo)
    {
        return propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string);
    }
}
