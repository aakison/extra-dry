namespace ExtraDry.Swashbuckle;

/// <summary>
/// Helper class to provide renaming options for SchemaIds for Swagger.
/// </summary>
public static class SchemaIds {

    /// <summary>
    /// Rename interfaces that used to not indicate the obligatory "I" in front of each.
    /// </summary>
    public static string RenameInterfaces(Type type)
    {
        if(type.IsGenericType) {
            return $"{SimpleTypeName(type.GenericTypeArguments[0])}{SimpleTypeName(type)}";
        }
        else {
            return SimpleTypeName(type);
        }
    }

    private static string SimpleTypeName(Type type)
    {
        var name = type.Name;
        if(type.IsInterface && type.Name.StartsWith('I')) {
            name = name[1..];
        }
        if(type.IsGenericType) {
            name = name.Split('`')[0];
        }
        return name;
    }

}
