namespace ExtraDry.Server.Internal;

[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
public class JsonInterfaceConverterAttribute : JsonConverterAttribute {
    public JsonInterfaceConverterAttribute(Type converterType)
        : base(converterType)
    {
    }
}
