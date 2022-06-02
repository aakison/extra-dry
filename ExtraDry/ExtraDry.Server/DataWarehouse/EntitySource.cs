using System.Reflection;

namespace ExtraDry.Server.DataWarehouse;


public class EntitySource {

    public EntitySource(Type type)
    {
        EntityType = type;
    }

    [JsonIgnore]
    public Type? ContextType { get; set; }

    [JsonIgnore]
    public PropertyInfo? PropertyInfo { get; set; }

    [JsonIgnore]
    public Type EntityType { get; private init; }

    public string Reference => $"{ContextType?.Name}/{PropertyInfo?.Name}<{EntityType.Name}>";

}

