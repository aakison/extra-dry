using System.Reflection;

namespace ExtraDry.Core.Parser.Internal;

public class SortProperty(
    PropertyInfo property,
    string externalName,
    FilterAttribute? filter = null)
{
    public PropertyInfo Property { get; } = property;

    public string ExternalName { get; } = externalName;

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
