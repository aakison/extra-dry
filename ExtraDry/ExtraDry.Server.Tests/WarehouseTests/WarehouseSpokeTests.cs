using ExtraDry.Core.DataWarehouse;
using ExtraDry.Server.DataWarehouse;

namespace ExtraDry.Server.Tests.WarehouseTests;

public class WarehouseSpokeTests
{
    [Fact]
    public void ForeignKeyConstraints()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<TestContext>();

        var warehouse = builder.Build();

        var fact = warehouse.Facts.First(e => e.EntityType == typeof(TestContext.SimpleSource));
        Assert.Contains(fact.Columns, e => e.Name == "Target ID");
    }

    [Theory]
    [InlineData("First Target ID")]
    [InlineData("Second Reference Target ID")] // compound property name
    [InlineData("Third Reference Target ID")] // Ignore display attribute
    public void ForeignKeyConstraintsMultipleTargetNaming(string spokeName)
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<TestContext>();

        var warehouse = builder.Build();

        var fact = warehouse.Facts.First(e => e.EntityType == typeof(TestContext.MultipleTarget));
        Assert.Contains(fact.Columns, e => e.Name == spokeName);
    }

    [Fact]
    public void SpokeIgnoreRemovesSpoke()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<TestContext>();

        var warehouse = builder.Build();

        var fact = warehouse.Facts.First(e => e.EntityType == typeof(TestContext.Spokeless));
        Assert.DoesNotContain(fact.Columns, e => e.Name == "Target ID");
    }

    [Fact]
    public void FluentCanChangeFactSpokeName()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<TestContext>();

        builder.Fact<TestContext.SimpleSource>().Spoke(e => e.TargetName).HasName("Changed Name");
        var warehouse = builder.Build();

        var fact = warehouse.Facts.First(e => e.EntityType == typeof(TestContext.SimpleSource));
        Assert.Contains(fact.Columns, e => e.Name == "Changed Name");
        Assert.DoesNotContain(fact.Columns, e => e.Name == "Target ID");
    }

    [Fact]
    public void FluentCanChangeDimensionSpokeName()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<TestContext>();

        builder.Dimension<TestContext.DimensionSource>().Spoke(e => e.Target).HasName("Changed Name");
        var warehouse = builder.Build();

        var dimension = warehouse.Dimensions.First(e => e.EntityType == typeof(TestContext.DimensionSource));
        Assert.Contains(dimension.Columns, e => e.Name == "Changed Name");
        Assert.DoesNotContain(dimension.Columns, e => e.Name == "Target ID");
    }

    [Fact]
    public void DimensionForeignKeyConstraints()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<TestContext>();

        var warehouse = builder.Build();

        var dimension = warehouse.Dimensions.First(e => e.EntityType == typeof(TestContext.DimensionSource));
        Assert.Contains(dimension.Columns, e => e.Name == "Target ID");
    }

    [Fact]
    public void DoubleDutyFactDimensionTableSpokesOnFactOnly()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<TestContext>();

        var warehouse = builder.Build();

        var fact = warehouse.Facts.First(e => e.EntityType == typeof(TestContext.DoubleDutySource));
        var dimension = warehouse.Dimensions.First(e => e.EntityType == typeof(TestContext.DoubleDutySource));
        Assert.Contains(fact.Columns, e => e.Name == "Target ID");
        Assert.DoesNotContain(dimension.Columns, e => e.Name == "Target ID");
    }

    [Fact]
    public void SelfReferentialDimension()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<SelfReferenceContext>();

        var warehouse = builder.Build();

        var dimension = warehouse.Dimensions.First(e => e.EntityType == typeof(SelfReferenceContext.SelfReference));
        Assert.Contains(dimension.Columns, e => e.Name.EndsWith("Self Reference ID", StringComparison.Ordinal));
    }

    public class TestContext : DbContext
    {
        [FactTable]
        public class SimpleSource
        {
            [JsonIgnore]
            public int Id { get; set; }

            public Target? TargetName { get; set; }
        }

        [FactTable]
        public class MultipleTarget
        {
            [JsonIgnore]
            public int Id { get; set; }

            public Target? First { get; set; }

            public Target? SecondReference { get; set; }

            [Display(Name = "Thirdly")] // ignored for spoke generation
            public Target? ThirdReference { get; set; }
        }

        [FactTable]
        public class Spokeless
        {
            [JsonIgnore]
            public int Id { get; set; }

            [SpokeIgnore]
            public Target? Taret { get; set; }
        }

        [DimensionTable]
        public class Target
        {
            [JsonIgnore]
            public int Id { get; set; }
        }

        [DimensionTable]
        public class DimensionSource
        {
            [JsonIgnore]
            public int Id { get; set; }

            public Target? Target { get; set; } // dimensions can also have spokes (snowflake schema)
        }

        [FactTable, DimensionTable]
        public class DoubleDutySource
        {
            [JsonIgnore]
            public int Id { get; set; }

            public Target? Target { get; set; } // dimensions can also have spokes (snowflake schema)
        }

        public DbSet<SimpleSource> SimpleSources { get; set; } = null!;

        public DbSet<MultipleTarget> DoubleTargets { get; set; } = null!;

        public DbSet<Spokeless> Spokelesses { get; set; } = null!;

        public DbSet<Target> Targets { get; set; } = null!;

        public DbSet<DimensionSource> DimensionSources { get; set; } = null!;

        public DbSet<DoubleDutySource> DoubleDutySources { get; set; } = null!;
    }

    public class SelfReferenceContext : DbContext
    {
        [DimensionTable]
        public class SelfReference
        {
            [JsonIgnore]
            public int Id { get; set; }

            public SelfReference? RelatedSelfReference { get; set; }
        }

        public DbSet<SelfReference> SelfReferences { get; set; } = null!;
    }
}
