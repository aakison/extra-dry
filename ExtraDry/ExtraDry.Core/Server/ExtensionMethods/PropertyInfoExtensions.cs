using System.Reflection;
using System.Text.Json.Serialization;

namespace ExtraDry.Server {
    public static class PropertyInfoExtensions {
        public static bool IsValueOrImmutable(this PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string);
        }

        public static bool IsJsonIgnored(this PropertyInfo propertyInfo)
        {
            var ignoreAttribute = propertyInfo.GetCustomAttribute<JsonIgnoreAttribute>();
            return ignoreAttribute != null && ignoreAttribute.Condition == JsonIgnoreCondition.Always;
        }
    }
}
