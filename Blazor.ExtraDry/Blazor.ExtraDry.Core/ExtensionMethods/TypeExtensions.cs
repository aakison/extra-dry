using System;

namespace Blazor.ExtraDry.Core.Tests.ExtensionMethods {
    public static class TypeExtensions {
        public static object GetDefaultValue(this Type type)
        {
            if(type.IsValueType && Nullable.GetUnderlyingType(type) == null) {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}
