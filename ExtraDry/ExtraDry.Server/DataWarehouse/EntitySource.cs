using System.Reflection;

namespace ExtraDry.Server.DataWarehouse;


public class EntitySource {

    public EntitySource(Type type)
    {
        EntityType = type;
    }

    public Type? ContextType { get; set; }

    public PropertyInfo? PropertyInfo { get; set; }

    public Type EntityType { get; private init; }

}

