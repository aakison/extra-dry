namespace ExtraDry.Core;

/// <summary>
/// Resolves an ExpandoSchema on the server for a given type. Only one Expando Schema should be
/// registered for each object type.
/// </summary>
public interface IExpandoSchemaResolver
{
    /// <summary>
    /// Given a target object, return the ExpandoSchema that defines the custom fields for the
    /// object.
    /// </summary>
    Task<ExpandoSchema?> ResolveAsync(object target);
}
