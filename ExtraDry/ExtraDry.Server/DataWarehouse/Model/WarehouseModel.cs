using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace ExtraDry.Server.DataWarehouse;


public class WarehouseModel {

    internal WarehouseModel(WarehouseModelBuilder builder, Type entityContextType, string? group = null)
    {
        EntityContextType = entityContextType;
        Group = group;
        DimensionTables = new List<Table>(builder.BuildDimensions());
        FactTables = new List<Table>(builder.BuildFacts());
    }

    public WarehouseModel(Type entityContextType, string group) : this(entityContextType, null, group)
    {

    }

    public WarehouseModel(Type entityContextType, Action<WarehouseModelBuilder>? onCreating = null, string? group = null)
    {
        EntityContextType = entityContextType;
        Group = group;

        var builder = new WarehouseModelBuilder();
        builder.LoadSchema(entityContextType);
        onCreating?.Invoke(builder);
        OnCreating(builder);

        DimensionTables = new List<Table>(builder.BuildDimensions());
        FactTables = new List<Table>(builder.BuildFacts());
    }

    protected virtual void OnCreating(WarehouseModelBuilder builder)
    {
        // no-op, for overloading in derived classes.
    }

    public Type EntityContextType { get; }

    public string? Group { get; }

    public ReadOnlyCollection<Table> Facts => facts ??= new ReadOnlyCollection<Table>(FactTables);
    private ReadOnlyCollection<Table>? facts;

    public ReadOnlyCollection<Table> Dimensions => dimensions ??= new ReadOnlyCollection<Table>(DimensionTables);
    private ReadOnlyCollection<Table>? dimensions;

    private List<Table> DimensionTables { get; }

    private List<Table> FactTables { get; }

}

public class WarehouseModel<TOltpContext> : WarehouseModel
    where TOltpContext : DbContext {

    public WarehouseModel(string? group = null) 
        : base(typeof(TOltpContext), null, group)
    { 
    }

    public WarehouseModel(Action<WarehouseModelBuilder> builder, string? group = null) 
        : base(typeof(TOltpContext), builder, group)
    {
    }

}
