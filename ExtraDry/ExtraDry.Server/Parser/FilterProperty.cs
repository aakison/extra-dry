using System.Reflection;

namespace ExtraDry.Server.Internal;

/// <summary>
/// Encapsulates a property that has the `FilterAttribute` on it.
/// </summary>
internal class FilterProperty(
    PropertyInfo property,
    FilterAttribute filter,
    string? externalName = null)
{
    public PropertyInfo Property { get; set; } = property;

    public FilterAttribute Filter { get; set; } = filter;

    public string ExternalName { get; } = externalName ?? property.Name;

}
