
namespace ExtraDry.Core;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class InputFormatAttribute : Attribute
{
    public Type? DataTypeOverride { get; set; }
}
