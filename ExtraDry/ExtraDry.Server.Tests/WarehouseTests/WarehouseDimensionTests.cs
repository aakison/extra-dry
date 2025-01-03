using ExtraDry.Server.DataWarehouse;

namespace ExtraDry.Server.Tests.WarehouseTests;

public class WarehouseDimensionTests
{
    [Theory]
    [InlineData(typeof(Company), "Company Details")] // Both fact and dimension, so get "Details" appended.
    [InlineData(typeof(Region), "Geographic Region")] // explicit name.
    public void EntityBecomesDimension(Type entityType, string title)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var warehouse = builder.Build();

        Assert.Contains(warehouse.Dimensions, e => e.EntityType == entityType && e.Name == title);
    }

    [Theory]
    [InlineData(typeof(Employee))]
    public void EntityDoesNotBecomeDimension(Type entityType)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var warehouse = builder.Build();

        Assert.DoesNotContain(warehouse.Dimensions, e => e.EntityType == entityType);
    }

    [Theory]
    [InlineData(typeof(Region), "Geographic Region ID", ColumnType.Key)] // Key Column naming convention
    [InlineData(typeof(Region), "Slug", ColumnType.Text)] // Simple property, no decoration or special handling
    [InlineData(typeof(Region), "Compound Name", ColumnType.Text)] // Simple property, name title cased by default.
    [InlineData(typeof(Region), "The Title", ColumnType.Text)] // Dimension attribute rename
    public void DimensionHasColumn(Type entityType, string title, ColumnType columnType)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var warehouse = builder.Build();

        var fact = warehouse.Dimensions.Single(e => e.EntityType == entityType);
        Assert.Contains(fact.Columns, e => e.Name == title && e.ColumnType == columnType);
    }

    [Theory]
    [InlineData(typeof(Region), "Population")] // Regular integer column
    [InlineData(typeof(Region), "Password")] // AttributeIgnore'd
    public void FactDoesNotHaveColumn(Type entityType, string title)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var warehouse = builder.Build();

        var fact = warehouse.Dimensions.Single(e => e.EntityType == entityType);
        Assert.DoesNotContain(fact.Columns, e => e.Name == title);
    }

    [Theory]
    [InlineData(typeof(Region))]
    public void DimensionHasNoData(Type entityType)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var warehouse = builder.Build();

        var fact = warehouse.Dimensions.Single(e => e.EntityType == entityType);
        Assert.Empty(fact.Data);
    }

    [Fact]
    public void MethodExpressionThrowsException()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<SampleContext>();

        Assert.Throws<DryException>(() => builder.Dimension<Region>().Attribute(e => e.GetHashCode()));
    }

    [Fact]
    public void FieldMemberExpressionThrowsException()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<SampleContext>();

        Assert.Throws<DryException>(() => builder.Dimension<Company>().Attribute(e => e.field));
    }
}
