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

    [Fact]
    public void MissingEnumThrowsException()
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        Assert.Throws<DryException>(() => builder.EnumDimension<RegionLevel>());
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

    [Theory]
    [InlineData(typeof(RegionStatus))]
    [InlineData(typeof(CompanyStatus))]
    public void KeysAreAlwaysIncluded(Type type)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();

        Assert.NotNull(builder.EnumDimension(type).HasKey());
    }

    [Theory]
    [InlineData(typeof(RegionStatus), nameof(EnumDimension.ShortName), true)]
    [InlineData(typeof(RegionStatus), nameof(EnumDimension.Description), true)]
    [InlineData(typeof(RegionStatus), nameof(EnumDimension.Order), false)]
    [InlineData(typeof(RegionStatus), nameof(EnumDimension.GroupName), false)]
    [InlineData(typeof(RegionStatus), nameof(EnumDimension.Name), true)]
    [InlineData(typeof(CompanyStatus), nameof(EnumDimension.ShortName), false)]
    [InlineData(typeof(CompanyStatus), nameof(EnumDimension.Description), false)]
    [InlineData(typeof(CompanyStatus), nameof(EnumDimension.Order), true)]
    [InlineData(typeof(CompanyStatus), nameof(EnumDimension.GroupName), true)]
    [InlineData(typeof(CompanyStatus), nameof(EnumDimension.Name), true)]
    public void AttributesAreConditionallyIncluded(Type type, string name, bool included)
    {
        var builder = new WarehouseModelBuilder();
        
        builder.LoadSchema<SampleContext>();

        Assert.Equal(included, builder.EnumDimension(type).Attribute(name).Included);
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

        Assert.Throws<DryException>(() => builder.EnumDimension<RegionStatus>().Attribute(e => e.GetHashCode()));
    }


}
