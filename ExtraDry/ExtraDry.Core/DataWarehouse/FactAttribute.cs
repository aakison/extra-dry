namespace ExtraDry.Core.DataWarehouse;

[AttributeUsage(AttributeTargets.Class)]
public class FactAttribute : Attribute {

    public FactAttribute() { }

    public FactAttribute(string name) { 
        Name = name;
    }

    public string? Name { get; set; }

}
