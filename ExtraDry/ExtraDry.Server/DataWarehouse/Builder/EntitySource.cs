using System.Reflection;

namespace ExtraDry.Server.DataWarehouse;

/// <summary>
/// The reference to a source of an entity for checking for changes requiring updates to the data warehouse.
/// </summary>
public class EntitySource {

    /// <summary>
    /// Creates a EntitySource, only the entity type is required.
    /// If using a DbContext as the source, also populate the ContextType and PropertyInfo.
    /// </summary>
    public EntitySource(Type type)
    {
        EntityType = type;
    }

    /// <summary>
    /// The context that is used to store the entity, typically a subclass of DbContext.
    /// If source is an Enum, then there is no context and this is null.
    /// </summary>
    [JsonIgnore]
    public Type? ContextType { get; set; }

    /// <summary>
    /// For sources within a DbContext, references the DbSet`T property.
    /// </summary>
    [JsonIgnore]
    public PropertyInfo? PropertyInfo { get; set; }

    /// <summary>
    /// The type of the entity, typically from a DbContext and contained in a DbSet property.
    /// </summary>
    [JsonIgnore]
    public Type EntityType { get; private init; }

    /// <summary>
    /// For schema checking, a serialized snapshot of the above properties.
    /// </summary>
    public string Reference => $"{ContextType?.Name}/{PropertyInfo?.Name}<{EntityType.Name}>";

}

