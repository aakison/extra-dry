using System.Reflection;

namespace ExtraDry.Server.Internal;

internal class SortProperty(
    PropertyInfo property, 
    string externalName)
{
    public PropertyInfo Property { get; } = property;

    public string ExternalName { get; } = externalName;
}
