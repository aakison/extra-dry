using System.Reflection;

namespace ExtraDry.Core.Parser.Internal;

public class SortProperty(
    PropertyInfo property,
    string externalName,
    FilterAttribute? filter = null,
    string? alternateName = null)
{
    public PropertyInfo Property { get; } = property;

    public string ExternalName { get; } = externalName;

    /// <summary>
    /// The name of the property when it is aliased by `FilterAttribute.Name`, which is also accepted
    /// when matching a query to this property.
    /// </summary>
    public string? AlternateName { get; } = alternateName;

    /// <summary>
    /// Determines if the name used in a query refers to this property.
    /// </summary>
    public bool IsNamed(string name) =>
        string.Equals(ExternalName, name, StringComparison.OrdinalIgnoreCase)
        || string.Equals(AlternateName, name, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// The chain of properties to traverse to reach the value that is sorted. Is just the
    /// property itself unless the filter attribute specifies a `Path`.
    /// </summary>
    public PropertyInfo[] QueryPath { get; } = FilterProperty.ResolvePath(property, filter);

    /// <summary>
    /// The type of the value that is sorted, i.e. the type of the last property in the path.
    /// </summary>
    public Type ValueType => QueryPath[^1].PropertyType;
}
