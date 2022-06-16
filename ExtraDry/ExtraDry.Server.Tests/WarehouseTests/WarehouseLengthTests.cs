using ExtraDry.Core.DataWarehouse;
using ExtraDry.Server.DataWarehouse;
using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Server.Tests.WarehouseTests;

public class WarehouseLengthTests {

    [Theory]
    [InlineData("Uuid", 36)] // Simple Guid property, get basic length
    [InlineData("LargerMaxGuid", 40)] // OK to re-state larger length
    [InlineData("SmallerStringGuid", 30)] // OK to re-state smaller length
    [InlineData("DefaultName", null)] // Not stated, get's null which imlies varchar(max)
    [InlineData("MaxLengthName", 103)]
    [InlineData("StringLengthName", 104)]
    [InlineData("Uri", 2083)]
    [InlineData("Thumbnail1", 101)]
    [InlineData("Thumbnail2", 102)]
    [InlineData("ChristianName", 50)]
    public void PropertyLengths(string name, int? length)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<AttributeContext>();
        var warehouse = builder.Build();

        var dimension = warehouse.Dimensions.Single(e => e.EntityType == typeof(AttributeContainer));
        var attribute = dimension.Columns.First(e => e.Name == DataConverter.CamelCaseToTitleCase(name));
        Assert.Equal(length, attribute.Length);
    }

    [Fact]
    public void NegativeLengthException()
    {
        var builder = new WarehouseModelBuilder();

        Assert.Throws<DryException>(() => builder.LoadSchema<BadLengthContext>());
    }

    [Theory]
    [InlineData(20)]
    [InlineData(null)]
    public void FluentLengthSetter(int? length)
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<AttributeContext>();

        builder.Dimension<AttributeContainer>().Attribute(e => e.MaxLengthName).HasLength(length);

        var warehouse = builder.Build();
        var dimension = warehouse.Dimensions.Single(e => e.EntityType == typeof(AttributeContainer));
        var attribute = dimension.Columns.First(e => e.Name == DataConverter.CamelCaseToTitleCase("MaxLengthName"));
        Assert.Equal(length, attribute.Length);
    }

    public class AttributeContext : DbContext {
        public DbSet<AttributeContainer> Attributes { get; set; } = null!;
    }

    [DimensionTable]
    public class AttributeContainer {

        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        // Default is 36
        public Guid Uuid { get; set; }

        [MaxLength(40)] // extra space OK in case other stuff in the DW is merged?
        public Guid LargerMaxGuid { get; set; }

        [StringLength(30)] // less space is OK in case want to get most benefit of GUID, but maybe not all...
        public Guid SmallerStringGuid { get; set; }


        // Should be varchar(max)
        public string DefaultName { get; set; } = string.Empty;

        [MaxLength(103)]
        public string MaxLengthName { get; set; } = string.Empty;

        [StringLength(104)]
        public string StringLengthName { get; set; } = string.Empty;


        // Default is 2083 chars
        public Uri Uri { get; set; } = null!;

        [MaxLength(101)]
        public Uri Thumbnail1 { get; set; } = null!;

        [StringLength(102)]
        public Uri Thumbnail2 { get; set; } = null!;

        [StringLength(50)]
        public string ChristianName {
            get => DefaultName;
        }

    }

    public class BadLengthContext : DbContext {

        [DimensionTable]
        public class BadClass {

            [Key]
            [JsonIgnore]
            public int Id { get; set; }

            [StringLength(-1)]
            public string Name { get; set; } = string.Empty;

        }

        public DbSet<BadClass> BadLengthClasses { get; set; } = null!;
    }

}
