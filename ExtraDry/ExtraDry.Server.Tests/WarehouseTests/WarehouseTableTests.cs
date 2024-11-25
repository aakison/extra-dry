using ExtraDry.Core.DataWarehouse;
using ExtraDry.Server.DataWarehouse;

namespace ExtraDry.Server.Tests.WarehouseTests;

public class WarehouseTableTests {

    [Theory]
    [InlineData(typeof(FactTableCollisionContext))]
    [InlineData(typeof(DimensionTableCollisionContext))]
    [InlineData(typeof(MixedTableCollisionContext))]
    public void ContextFactNameCollision(Type type)
    {
        var builder = new WarehouseModelBuilder();

        Assert.Throws<DryException>(() => builder.LoadSchema(type));
    }

    [Theory]
    [InlineData("")] // empty
    [InlineData("   ")] // blank
    [InlineData("0123456789012345678901234567890123456789012345678901")] // too long
    [InlineData("Worker Bees")] // collides with Employee fact
    [InlineData("Geographic Region")] // collides with Region dimension
    public void InvalidFluentFactNameChange(string name)
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<SampleContext>();

        Assert.Throws<DryException>(() => builder.Fact<Company>().HasName(name));
    }

    [Theory]
    [InlineData("")] // empty
    [InlineData("   ")] // blank
    [InlineData("0123456789012345678901234567890123456789012345678901")] // too long
    [InlineData("Worker Bees")] // collides with Employee fact
    [InlineData("Geographic Region")] // collides with Region dimension
    public void InvalidFluentDimensionNameChange(string name)
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<SampleContext>();

        Assert.Throws<DryException>(() => builder.Dimension<Company>().HasName(name));
    }

    [Theory]
    [InlineData("New and Unused")] // any non colliding name
    [InlineData("Company")] // redundant, but OK
    public void ValidFluentFactNameChange(string name)
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<SampleContext>();

        builder.Fact<Company>().HasName(name);

        Assert.Equal(name, builder.Fact<Company>().TableName);
    }

    [Theory]
    [InlineData("New and Unused")] // any non colliding name
    [InlineData("Company Details")] // redundant, but OK
    public void ValidFluentDimensionNameChange(string name)
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<SampleContext>();

        builder.Dimension<Company>().HasName(name);

        Assert.Equal(name, builder.Dimension<Company>().TableName);
    }

    [Fact]
    public void ExceptionOnNoFactTable()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<SampleContext>();

        var ex = Assert.Throws<DryException>(() => builder.Fact<Region>());
    }

    [Fact]
    public void ExceptionOnNoDimensionTable()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<SampleContext>();

        var ex = Assert.Throws<DryException>(() => builder.Dimension<Employee>());
    }

    public class FactTableCollisionContext : DbContext {

        [FactTable]
        public class FirstTable {
            [JsonIgnore]
            public int Id { get; set; }
        }

        [FactTable("First Table")]
        public class SecondTable {
            [JsonIgnore]
            public int Id { get; set; }
        }

        public DbSet<FirstTable> Firsts { get; set; } = null!;

        public DbSet<SecondTable> Seconds { get; set; } = null!;

    }

    public class DimensionTableCollisionContext : DbContext {

        [DimensionTable]
        public class FirstTable {
            [JsonIgnore]
            public int Id { get; set; }
        }

        [DimensionTable("First Table")]
        public class SecondTable {
            [JsonIgnore]
            public int Id { get; set; }
        }

        public DbSet<FirstTable> Firsts { get; set; } = null!;

        public DbSet<SecondTable> Seconds { get; set; } = null!;

    }

    public class MixedTableCollisionContext : DbContext {

        [FactTable]
        public class FirstTable {
            [JsonIgnore]
            public int Id { get; set; }
        }

        [DimensionTable("First Table")]
        public class SecondTable {
            [JsonIgnore]
            public int Id { get; set; }
        }

        public DbSet<FirstTable> Firsts { get; set; } = null!;

        public DbSet<SecondTable> Seconds { get; set; } = null!;

    }
}
