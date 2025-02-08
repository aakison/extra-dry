using System.Reflection;

namespace ExtraDry.Core.Parser.Internal;

/// <summary>
/// Encapsulates a property that has the `FilterAttribute` on it.
/// </summary>
public class FilterProperty(
    PropertyInfo property,
    FilterAttribute filter,
    string? externalName = null)
{
    public PropertyInfo Property { get; set; } = property;

    public FilterAttribute Filter { get; set; } = filter;

    public string ExternalName { get; } = externalName ?? property.Name;
}
