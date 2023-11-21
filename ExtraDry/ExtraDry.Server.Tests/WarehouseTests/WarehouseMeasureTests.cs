using ExtraDry.Core.DataWarehouse;
using ExtraDry.Server.DataWarehouse;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Server.Tests.WarehouseTests;

public class WarehouseMeasureTests {

    [Theory]
    [InlineData("Measure Container ID", ColumnType.Key)] // Key Column naming convention
    [InlineData(nameof(MeasureContainer.Integer), ColumnType.Integer)] // Simple int property, no decoration or special handling
    [InlineData(nameof(MeasureContainer.Short), ColumnType.Integer)] // Simple short property, no decoration or special handling
    [InlineData(nameof(MeasureContainer.Float), ColumnType.Real)] // Simple float property, no decoration or special handling
    [InlineData(nameof(MeasureContainer.Double), ColumnType.Real)] // Simple double property, no decoration or special handling
    [InlineData(nameof(MeasureContainer.Gross), ColumnType.Decimal)] // Simple decimal property, no decoration or special handling
    [InlineData("Annual Revenue", ColumnType.Decimal)] // Simple decimal property, with naming conversion
    [InlineData("Double Revenue", ColumnType.Decimal)] // Decimal property with getter only, with naming conversion
    [InlineData(nameof(MeasureContainer.Gifts), ColumnType.Decimal)] // NotMapped, but also Measure so include it.
    [InlineData("Big Bucks", ColumnType.Decimal)] // Measure attribute rename
    public void MeasureIncluded(string title, ColumnType columnType)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<MeasureContext>();
        var warehouse = builder.Build();

        var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
        Assert.Contains(fact.Columns, e => e.Name == title && e.ColumnType == columnType);
    }

    [Theory]
    [InlineData("ID")] // Key property doesn't slip through as measure.
    [InlineData("Name")] // String column not a measure.
    [InlineData("Sales")] // EF NotMappedAttribue suppresses by default.
    [InlineData("Ignored")] // MeasureIgnoreAttribue suppresses.
    public void MeasureNotIncluded(string title)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<MeasureContext>();
        var warehouse = builder.Build();

        var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
        Assert.DoesNotContain(fact.Columns, e => e.Name == title);
    }

    [Fact]
    public void FluentRenameOfMeasure()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<MeasureContext>();

        builder.Fact<MeasureContainer>().Measure(e => e.GrossSalesLessCOGS).HasName("Gross Margin");

        var warehouse = builder.Build();
        var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
        Assert.Contains(fact.Columns, e => e.Name == "Gross Margin" && e.ColumnType == ColumnType.Decimal);
    }

    [Theory]
    [InlineData(typeof(EmptyMeasureNameContext))]
    [InlineData(typeof(BlankMeasureNameContext))]
    public void BadMeasureNameThrowsException(Type type)
    {
        var builder = new WarehouseModelBuilder();
        Assert.Throws<DryException>(() => builder.LoadSchema(type));
    }

    [Theory]
    [InlineData("0123456789012345678901234567890123456789012345678901")]
    [InlineData("")]
    [InlineData("     ")]
    public void BadMeasureNameInFluent(string name)
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<MeasureContext>();

        Assert.Throws<DryException>(() => builder.Fact<MeasureContainer>().Measure(e => e.GrossSalesLessCOGS).HasName(name));
    }

    [Theory]
    [InlineData("012345678901234567890123456789012345678901234567")]
    [InlineData("!!!")]
    [InlineData("!@#$")]
    public void WackyButValidMeasureNameInFluent(string name)
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<MeasureContext>();

        builder.Fact<MeasureContainer>().Measure(e => e.GrossSalesLessCOGS).HasName(name);

        var warehouse = builder.Build();
        var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
        Assert.Contains(fact.Columns, e => e.Name == name);
    }

    [Fact]
    public void CanChangeToValidTypeViaFluent()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<MeasureContext>();

        builder.Fact<MeasureContainer>().Measure(e => e.Gross).HasColumnType(ColumnType.Real);

        var warehouse = builder.Build();
        var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
        Assert.Contains(fact.Columns, e => e.Name == "Gross" && e.ColumnType == ColumnType.Real);
    }

    [Fact]
    public void CannotChangeToInvalidTypeViaFluent()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<MeasureContext>();

        Assert.Throws<DryException>(() => builder.Fact<MeasureContainer>().Measure(e => e.Gross).HasColumnType(ColumnType.Text));
    }

    [Fact]
    public void CannotHaveDuplicateMeasureNames()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<MeasureContext>();

        Assert.Throws<DryException>(() => builder.Fact<MeasureContainer>().Measure(e => e.Gross).HasName("Big Bucks"));
    }

    [Fact]
    public void FluentCanExplicitlyIgnoreMeasure()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<MeasureContext>();

        builder.Fact<MeasureContainer>().Measure(e => e.AnnualRevenue).IsIncluded(false);
        var warehouse = builder.Build();

        var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
        Assert.DoesNotContain(fact.Columns, e => e.Name == "Annual Revenue");
    }

    [Fact]
    public void FluentCanUnIgnoreMeasure()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<MeasureContext>();

        builder.Fact<MeasureContainer>().Measure(e => e.Ignored).IsIncluded(true);
        var warehouse = builder.Build();

        var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
        Assert.Contains(fact.Columns, e => e.Name == "Ignored");
    }

    [Fact]
    public void DuplicateNamesByConventionException()
    {
        var builder = new WarehouseModelBuilder();

        Assert.Throws<DryException>(() => builder.LoadSchema<NameCollisionContext>());
    }

    [Fact]
    public void DuplicateNamesByFluentException()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<MeasureContext>();

        Assert.Throws<DryException>(() => builder.Fact<MeasureContainer>().Measure(e => e.Short).HasName("Integer"));
    }

    [Fact]
    public void DefaultPrecision()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<PrecisionContext>();

        var warehouse = builder.Build();

        var fact = warehouse.Facts.Single(e => e.EntityType == typeof(PrecisionContext.PrecisionClass));
        var measure = fact.Columns.First(e => e.Name == "Default");
        Assert.Contains("18,2", measure.Precision);
    }

    [Theory]
    [InlineData("Short Size", "6,2")]
    [InlineData("Long Scale", "18,6")]
    [InlineData("Missing Scale", "20,2")] // default scale is 2 (matching EF default scale, not C# or database)
    public void AttributePrecision(string propertyName, string expected)
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<PrecisionContext>();

        var warehouse = builder.Build();

        var fact = warehouse.Facts.Single(e => e.EntityType == typeof(PrecisionContext.PrecisionClass));
        var measure = fact.Columns.First(e => e.Name == propertyName);
        Assert.Contains(expected, measure.Precision);
    }

    [Fact]
    public void FluentPrecision()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<PrecisionContext>();

        builder.Fact<PrecisionContext.PrecisionClass>().Measure(e => e.FluentScale).HasPrecision(5, 3);
        var warehouse = builder.Build();

        var fact = warehouse.Facts.Single(e => e.EntityType == typeof(PrecisionContext.PrecisionClass));
        var measure = fact.Columns.First(e => e.Name == "Fluent Scale");
        Assert.Contains("5,3", measure.Precision);
    }

    [Theory]
    [InlineData(40, 3)] // precision overflow
    [InlineData(2, 18)] // scale bigger than precision
    [InlineData(0, 0)] // precision too low
    [InlineData(18, -1)] // scale too low
    public void InvalidPrecisionForDecimal(int precision, int scale)
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<PrecisionContext>();

        Assert.Throws<DryException>(() => builder.Fact<PrecisionContext.PrecisionClass>().Measure(e => e.FluentScale).HasPrecision(precision, scale));
    }

}

public class MeasureContext : DbContext {

    public DbSet<MeasureContainer> Measures { get; set; } = null!;
}

[FactTable]
public class MeasureContainer {

    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    [Measure]
    public string Title { get; set; } = string.Empty;

    [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "For testing purposes.")]
    public int Integer { get; set; }

    [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "For testing purposes.")]
    public short Short { get; set; }

    [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "For testing purposes.")]
    public float Float { get; set; }

    [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "For testing purposes.")]
    public double Double { get; set; }

    public decimal Gross { get; set; }

    public decimal AnnualRevenue { get; set; }

    [Measure("Big Bucks")]
    public decimal BadName { get; set; }

    public decimal DoubleRevenue {
        get => 2 * AnnualRevenue;
    }

    [NotMapped]
    public decimal Sales { get; set; }

    [NotMapped, Measure]
    public decimal Gifts { get; set; }

    public decimal GrossSalesLessCOGS { get; set; }

    [MeasureIgnore]
    public decimal Ignored { get; set; }
}

public class EmptyMeasureNameContext : DbContext {

    [FactTable]
    public class BadClass {

        [Measure("")]
        public decimal GoodName { get; set; }

    }

    public DbSet<BadClass> BadClasses { get; set; } = null!;
}

public class BlankMeasureNameContext : DbContext {

    [FactTable]
    public class BadClass {

        [Measure("   ")]
        public decimal GoodName { get; set; }

    }

    public DbSet<BadClass> BadClasses { get; set; } = null!;
}

public class NameCollisionContext : DbContext {

    [FactTable]
    public class NameCollisionClass {

        public decimal Name { get; set; }

        [Measure("Name")]
        public decimal Title { get; set; }
    }

    public DbSet<NameCollisionClass> NameCollisionClasses { get; set; } = null!;
}

public class PrecisionContext : DbContext {

    [FactTable]
    public class PrecisionClass {

        [JsonIgnore]
        public int Id { get; set; }

        public decimal Default { get; set; }

        [Precision(6, 2)]
        public decimal ShortSize { get; set; }

        [Precision(18, 6)]
        public decimal LongScale { get; set; }

        [Precision(20)]
        public decimal MissingScale { get; set; }

        public decimal FluentScale { get; set; }

    }

    public DbSet<PrecisionClass> PrecisionClasses { get; set; } = null!;
}
