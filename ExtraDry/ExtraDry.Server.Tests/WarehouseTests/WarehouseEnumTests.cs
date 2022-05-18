using ExtraDry.Server.DataWarehouse;
using ExtraDry.Server.DataWarehouse.Builder;

namespace ExtraDry.Server.Tests.WarehouseTests;

public class WarehouseEnumTests {

    [Theory]
    [InlineData("Company Status")] // implied name.
    [InlineData("Geo Status")] // explicit name.
    public void EnumBecomesDimension(string title)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var warehouse = builder.Build();

        Assert.Contains(warehouse.Dimensions, e => e.EntityType == typeof(EnumDimension) && e.Name == title);
    }

    [Theory]
    [InlineData("Region Level")]
    public void EnumDoesNotBecomeDimension(string title)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var warehouse = builder.Build();

        Assert.DoesNotContain(warehouse.Dimensions, e => e.EntityType == typeof(EnumDimension) && e.Name == title);
    }

    [Theory]
    [InlineData("Name", ColumnType.Text)]
    [InlineData("Description", ColumnType.Text)]
    [InlineData("ShortName", ColumnType.Text)]
    [InlineData("GroupName", ColumnType.Text)]
    [InlineData("Order", ColumnType.Integer)]
    public void EnumBuilderHasAllAttributes(string title, ColumnType columnType)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();

        var attribute = builder.EnumDimension<RegionStatus>().Attribute(title);

        Assert.NotNull(attribute);
        Assert.Equal(columnType, attribute.ColumnType);
    }

    //[Theory]
    //[InlineData(typeof(Region), "Population")] // Regular integer column
    //[InlineData(typeof(Region), "Password")] // AttributeIgnore'd
    //public void FactDoesNotHaveColumn(Type entityType, string title)
    //{
    //    var builder = new WarehouseModelBuilder();

    //    builder.LoadSchema<SampleContext>();
    //    var warehouse = builder.Build();

    //    var fact = warehouse.Dimensions.Single(e => e.EntityType == entityType);
    //    Assert.DoesNotContain(fact.Columns, e => e.Name == title);
    //}

    //[Theory]
    //[InlineData(typeof(Region))]
    //public void DimensionHasNoData(Type entityType)
    //{
    //    var builder = new WarehouseModelBuilder();

    //    builder.LoadSchema<SampleContext>();
    //    var warehouse = builder.Build();

    //    var fact = warehouse.Dimensions.Single(e => e.EntityType == entityType);
    //    Assert.Empty(fact.Data);
    //}

    //[Fact]
    //public void MethodExpressionThrowsException()
    //{
    //    var builder = new WarehouseModelBuilder();
    //    builder.LoadSchema<SampleContext>();

    //    Assert.Throws<DryException>(() => builder.Dimension<Region>().Attribute(e => e.GetHashCode()));
    //}

    //[Fact]
    //public void FieldMemberExpressionThrowsException()
    //{
    //    var builder = new WarehouseModelBuilder();
    //    builder.LoadSchema<SampleContext>();

    //    Assert.Throws<DryException>(() => builder.Dimension<Company>().Attribute(e => e.field));
    //}

}
