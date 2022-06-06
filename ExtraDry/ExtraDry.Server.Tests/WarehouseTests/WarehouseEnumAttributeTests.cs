using ExtraDry.Server.DataWarehouse;
using ExtraDry.Server.DataWarehouse.Builder;

namespace ExtraDry.Server.Tests.WarehouseTests;

/// <summary>
/// Test for when an enum is simple attribute of an enclosing dimension.
/// Occurs when no [DimensionTable] on the enum.
/// </summary>
public class WarehouseEnumAttributeTests {

    [Fact]
    public void NonDimensionEnumBecomesAttribute()
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var attribute = builder.Dimension<Region>().Attribute(e => e.Level);

        Assert.NotNull(attribute);
        Assert.Equal("Level", attribute.ColumnName);
        Assert.True(attribute.Included);
        Assert.Equal(new EnumStats(typeof(RegionLevel)).DisplayNameMaxLength(), attribute.Length);
    }

    [Fact]
    public void NonDimensionEnumAttributeFluentName()
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var attribute = builder.Dimension<Region>().Attribute(e => e.Level)
            .HasName("Bob");

        Assert.NotNull(attribute);
        Assert.Equal("Bob", attribute.ColumnName);
    }

    [Fact]
    public void NonDimensionEnumAttributeFluentIncluded()
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var attribute = builder.Dimension<Region>().Attribute(e => e.Level)
            .IsIncluded(false);

        Assert.NotNull(attribute);
        Assert.False(attribute.Included);
    }

    [Fact]
    public void NonDimensionEnumAttributeFluentLength()
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<SampleContext>();
        var attribute = builder.Dimension<Region>().Attribute(e => e.Level)
            .HasLength(80);

        Assert.NotNull(attribute);
        Assert.Equal(80, attribute.Length);
    }

}
