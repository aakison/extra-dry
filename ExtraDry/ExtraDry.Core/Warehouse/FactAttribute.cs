using System;

namespace ExtraDry.Core.Warehouse;

[AttributeUsage(AttributeTargets.Class)]
public class FactAttribute : Attribute {

    public FactAttribute() { }

    public FactAttribute(string name) { 
        Name = name;
    }

    public string? Name { get; set; }

}
