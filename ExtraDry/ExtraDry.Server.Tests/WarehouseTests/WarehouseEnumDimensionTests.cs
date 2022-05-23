using ExtraDry.Server.DataWarehouse;
using ExtraDry.Server.DataWarehouse.Builder;

namespace ExtraDry.Server.Tests.WarehouseTests;

/// <summary>
/// Test for when an enum is a dimension.
/// Occurs when [DimensionTable] on the enum.
/// </summary>
public class WarehouseEnumDimensionTests {

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

    [Theory]
    [InlineData(typeof(RegionStatus), (int)RegionStatus.Active, nameof(EnumDimension.Description), "Region is active.")]
    [InlineData(typeof(RegionStatus), (int)RegionStatus.Active, nameof(EnumDimension.ShortName), "ACT")]
    [InlineData(typeof(CompanyStatus), (int)CompanyStatus.Active, nameof(EnumDimension.GroupName), "ForDisplay")]
    [InlineData(typeof(CompanyStatus), (int)CompanyStatus.Active, nameof(EnumDimension.Order), 123)]
    [InlineData(typeof(CompanyStatus), (int)CompanyStatus.Inactive, nameof(EnumDimension.Order), 10000)]
    [InlineData(typeof(CompanyStatus), (int)CompanyStatus.Deleted, nameof(EnumDimension.Order), 10000)]
    public void DimensionContainsData(Type type, int id, string attributeName, object value)
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<SampleContext>();

        var dimension = builder.EnumDimension(type);

        var key = dimension.HasKey();
        var data = builder.EnumDimension(type).Data;
        var attribute = dimension.Attribute(attributeName);
        var values = data.First(e => (int)e[key] == id);
        Assert.Equal(value, values[attribute]);
    }

    [Theory]
    [InlineData(typeof(RegionStatus), nameof(EnumDimension.Description), 59)] // "Region no longer exists, but is linked to historic records.".Length
    [InlineData(typeof(RegionStatus), nameof(EnumDimension.ShortName), 3)] // "ACT"
    [InlineData(typeof(CompanyStatus), nameof(EnumDimension.GroupName), 10)] // "ForDisplay"
    public void EnumBuilderImplicitLength(Type type, string attributeName, int length)
    {
        var builder = new WarehouseModelBuilder();
        
        builder.LoadSchema<SampleContext>();

        var descriptionAttribute = builder.EnumDimension(type).Attribute(attributeName);
        Assert.Equal(length, descriptionAttribute.Length);
    }

    [Fact]
    public void EnumBuilderFluentLength()
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var descriptionAttribute = builder.EnumDimension<RegionStatus>().Attribute(e => e.Description);
        descriptionAttribute.HasLength(80);

        Assert.Equal(80, descriptionAttribute.Length);
    }

}
