namespace ExtraDry.Server.Internal;

[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
public class JsonInterfaceConverterAttribute(
    Type converterType)
    : JsonConverterAttribute(converterType)
{
}
