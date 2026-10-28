using System.Linq.Expressions;
using System.Reflection;

namespace ExtraDry.Core.Parser.Internal;

/// <summary>
/// Encapsulates a property that has the `FilterAttribute` on it.
/// </summary>
public class FilterProperty(
    PropertyInfo property,
    FilterAttribute filter,
    string? externalName = null,
    string? alternateName = null)
{
    public PropertyInfo Property { get; set; } = property;

    public FilterAttribute Filter { get; set; } = filter;

    public string ExternalName { get; } = externalName ?? property.Name;

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
    /// The chain of properties to traverse to reach the value that is filtered. Is just the
    /// property itself unless the filter attribute specifies a `Path`.
    /// </summary>
    public PropertyInfo[] QueryPath { get; } = ResolvePath(property, filter);

    /// <summary>
    /// The type of the value that is filtered, i.e. the type of the last property in the path.
    /// </summary>
    public Type ValueType => QueryPath[^1].PropertyType;

    /// <summary>
    /// Builds the member access expression for the filtered value from the root parameter.
    /// </summary>
    public Expression BuildAccess(Expression root)
    {
        var expression = root;
        foreach(var step in QueryPath) {
            expression = Expression.Property(expression, step);
        }
        return expression;
    }

    internal static PropertyInfo[] ResolvePath(PropertyInfo property, FilterAttribute? filter)
    {
        if(string.IsNullOrWhiteSpace(filter?.Path)) {
            return [property];
        }
        var path = new List<PropertyInfo>();
        var type = property.ReflectedType ?? property.DeclaringType!;
        foreach(var name in filter.Path.Split('.')) {
            var step = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance)
                ?? throw new DryException($"Filter path `{filter.Path}` on property `{property.Name}` is invalid, could not find `{name}` on `{type.Name}`.");
            path.Add(step);
            type = step.PropertyType;
        }
        return [.. path];
    }
}
