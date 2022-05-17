using ExtraDry.Core.DataWarehouse;
using ExtraDry.Server.DataWarehouse;
using Microsoft.EntityFrameworkCore;

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

    //[Theory]
    //[InlineData(typeof(RegionStatus))]
    //[InlineData(typeof(RegionLevel))]
    //public void EntityDoesNotBecomeFact(Type entityType)
    //{
    //    var builder = new WarehouseModelBuilder();

    //    builder.LoadSchema<SampleContext>();
    //    var warehouse = builder.Build();

    //    Assert.DoesNotContain(warehouse.Facts, e => e.EntityType == entityType);
    //}

    //[Theory]
    //[InlineData(typeof(Company), "Company ID", ColumnType.Key)] // Key Column naming convention
    //[InlineData(typeof(Company), "Gross", ColumnType.Decimal)] // Simple property, no decoration or special handling
    //[InlineData(typeof(Company), "Sales Margin", ColumnType.Decimal)] // Simple property, name title cased by default.
    //[InlineData(typeof(Company), "Big Bucks", ColumnType.Decimal)] // Measure attribute rename
    ////[InlineData(typeof(Company), "Company Status ID", ColumnType.Integer)] // Dimension PK naming convention.
    //public void FactHasColumn(Type entityType, string title, ColumnType columnType)
    //{
    //    var builder = new WarehouseModelBuilder();

    //    builder.LoadSchema<SampleContext>();
    //    var warehouse = builder.Build();

    //    var fact = warehouse.Facts.Single(e => e.EntityType == entityType);
    //    Assert.Contains(fact.Columns, e => e.Name == title && e.ColumnType == columnType);
    //}

    //[Theory]
    //[InlineData(typeof(Company), "Title")] // Regular string column
    //[InlineData(typeof(Company), "Code")] // String column with Measure attribute.
    //public void FactDoesNotHaveColumn(Type entityType, string title)
    //{
    //    var builder = new WarehouseModelBuilder();

    //    builder.LoadSchema<SampleContext>();
    //    var warehouse = builder.Build();

    //    var fact = warehouse.Facts.Single(e => e.EntityType == entityType);
    //    Assert.DoesNotContain(fact.Columns, e => e.Name == title);
    //}

    //[Theory]
    //[InlineData(typeof(Company))]
    //public void FactHasNoData(Type entityType)
    //{
    //    var builder = new WarehouseModelBuilder();

    //    builder.LoadSchema<SampleContext>();
    //    var warehouse = builder.Build();

    //    var fact = warehouse.Facts.Single(e => e.EntityType == entityType);
    //    Assert.Empty(fact.Data);
    //}

    //[Fact]
    //public void MethodExpressionThrowsException()
    //{
    //    var builder = new WarehouseModelBuilder();
    //    builder.LoadSchema<SampleContext>();

    //    Assert.Throws<DryException>(() => builder.Fact<Company>().Measure(e => e.GetHashCode()));
    //}

    //[Fact]
    //public void FieldMemberExpressionThrowsException()
    //{
    //    var builder = new WarehouseModelBuilder();
    //    builder.LoadSchema<SampleContext>();

    //    Assert.Throws<DryException>(() => builder.Fact<Company>().Measure(e => e.field));
    //}

    public class FactTableCollisionContext : DbContext {

        [FactTable]
        public class FirstTable {
            public int Id { get; set; }
        }

        [FactTable("First Table")]
        public class SecondTable {
            public int Id { get; set; }
        }

        public DbSet<FirstTable> Firsts { get; set; } = null!;

        public DbSet<SecondTable> Seconds { get; set; } = null!;

    }

    public class DimensionTableCollisionContext : DbContext {

        [DimensionTable]
        public class FirstTable {
            public int Id { get; set; }
        }

        [DimensionTable("First Table")]
        public class SecondTable {
            public int Id { get; set; }
        }

        public DbSet<FirstTable> Firsts { get; set; } = null!;

        public DbSet<SecondTable> Seconds { get; set; } = null!;

    }

    public class MixedTableCollisionContext : DbContext {

        [FactTable]
        public class FirstTable {
            public int Id { get; set; }
        }

        [DimensionTable("First Table")]
        public class SecondTable {
            public int Id { get; set; }
        }

        public DbSet<FirstTable> Firsts { get; set; } = null!;

        public DbSet<SecondTable> Seconds { get; set; } = null!;

    }
}
