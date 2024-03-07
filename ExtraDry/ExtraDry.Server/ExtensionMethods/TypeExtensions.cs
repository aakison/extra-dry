namespace ExtraDry.Server;

public static class TypeExtensions {

    public static object? GetDefaultValue(this Type type)
    {
        if(type.IsValueType && Nullable.GetUnderlyingType(type) == null) {
            return Activator.CreateInstance(type);
        }

        return null;
    }

    public static bool IsTypeOf<T>(this Type type)
    {
        return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) ? type.GetGenericArguments()[0] : type) == typeof(T);
    }
}
