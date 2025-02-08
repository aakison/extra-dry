using System.Reflection;

namespace ExtraDry.Core.Parser.Internal;

public class SortProperty(
    PropertyInfo property,
    string externalName)
{
    public PropertyInfo Property { get; } = property;

    public string ExternalName { get; } = externalName;
}
