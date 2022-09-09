namespace ExtraDry.Blazor.Internal;

internal static class TypeExtensions {

    public static bool HasDefaultConstructor(this Type type) => type.GetConstructors().Any(t => t.GetParameters().Length == 0);

    public static Type SingleGenericType(this Type type)
    {
        var arguments = type.GetGenericArguments();
        if(arguments?.Length != 1) {
            throw new DryException("Generic type used that was expected to have a single type and didn't.", "Bad Type, please contact support. 0x0F67C23E");
        }
        return arguments.First();
    }

    public static object CreateDefaultInstance(this Type type)
    {
        if(!type.HasDefaultConstructor()) {
            throw new DryException("Generic type was expected to have a default constructor.", "Bad Type, please contact support. 0x0FA7CBDE");
        }
        var item = Activator.CreateInstance(type);
        if(item == null) {
            throw new DryException("Generic type failed to instantiate.", "Bad Type, please contact support. 0x0F21D0C2");
        }
        return item;
    }

}
