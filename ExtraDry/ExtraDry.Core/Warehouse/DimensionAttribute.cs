using System;

namespace ExtraDry.Core.Warehouse;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum)]
public class DimensionAttribute : Attribute {

    public DimensionAttribute() { }

    public DimensionAttribute(string name)
    {
        Name = name;
    }

    public string? Name { get; set; }

}
