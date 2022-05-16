using ExtraDry.Core.DataWarehouse;
using ExtraDry.Server.DataWarehouse;
using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Server.Tests.WarehouseTests;

public class WarehouseAttributeTests {

    //[Theory]
    //[InlineData("Measure Container ID", ColumnType.Key)] // Key Column naming convention
    //[InlineData("Integer", ColumnType.Integer)] // Simple int property, no decoration or special handling
    //[InlineData("Short", ColumnType.Integer)] // Simple short property, no decoration or special handling
    //[InlineData("Float", ColumnType.Double)] // Simple float property, no decoration or special handling
    //[InlineData("Double", ColumnType.Double)] // Simple double property, no decoration or special handling
    //[InlineData("Gross", ColumnType.Decimal)] // Simple decimal property, no decoration or special handling
    //[InlineData("Annual Revenue", ColumnType.Decimal)] // Simple decimal property, with naming conversion
    //[InlineData("Double Revenue", ColumnType.Decimal)] // Decimal property with getter only, with naming conversion
    //[InlineData("Gifts", ColumnType.Decimal)] // NotMapped, but also Measure so include it.
    //[InlineData("Big Bucks", ColumnType.Decimal)] // Measure attribute rename
    //public void MeasureIncluded(string title, ColumnType columnType)
    //{
    //    var builder = new WarehouseModelBuilder();

    //    builder.LoadSchema<MeasureContext>();
    //    var warehouse = builder.Build();

    //    var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
    //    Assert.Contains(fact.Columns, e => e.Name == title && e.ColumnType == columnType);
    //}

    //[Theory]
    //[InlineData("ID")] // Key property doesn't slip through as measure.
    //[InlineData("Name")] // String column not a measure.
    //[InlineData("Sales")] // EF NotMappedAttribue suppresses by default.
    //[InlineData("Ignored")] // MeasureIgnoreAttribue suppresses.
    //public void MeasureNotIncluded(string title)
    //{
    //    var builder = new WarehouseModelBuilder();

    //    builder.LoadSchema<MeasureContext>();
    //    var warehouse = builder.Build();

    //    var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
    //    Assert.DoesNotContain(fact.Columns, e => e.Name == title);
    //}

    //[Fact]
    //public void FluentRenameOfMeasure()
    //{
    //    var builder = new WarehouseModelBuilder();
    //    builder.LoadSchema<MeasureContext>();

    //    builder.Fact<MeasureContainer>().Measure(e => e.GrossSalesLessCOGS).HasName("Gross Margin");

    //    var warehouse = builder.Build();
    //    var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
    //    Assert.Contains(fact.Columns, e => e.Name == "Gross Margin" && e.ColumnType == ColumnType.Decimal);
    //}

    //[Theory]
    //[InlineData(typeof(EmptyMeasureNameContext))]
    //[InlineData(typeof(BlankMeasureNameContext))]
    //public void BadMeasureNameThrowsException(Type type)
    //{
    //    var builder = new WarehouseModelBuilder();
    //    Assert.Throws<DryException>(() => builder.LoadSchema(type));
    //}

    //[Theory]
    //[InlineData("0123456789012345678901234567890123456789012345678901")]
    //[InlineData("")]
    //[InlineData("     ")]
    //public void BadMeasureNameInFluent(string name)
    //{
    //    var builder = new WarehouseModelBuilder();
    //    builder.LoadSchema<MeasureContext>();

    //    Assert.Throws<DryException>(() => builder.Fact<MeasureContainer>().Measure(e => e.GrossSalesLessCOGS).HasName(name));
    //}

    //[Theory]
    //[InlineData("012345678901234567890123456789012345678901234567")]
    //[InlineData("!!!")]
    //[InlineData("!@#$")]
    //public void WackyButValidMeasureNameInFluent(string name)
    //{
    //    var builder = new WarehouseModelBuilder();
    //    builder.LoadSchema<MeasureContext>();

    //    builder.Fact<MeasureContainer>().Measure(e => e.GrossSalesLessCOGS).HasName(name);

    //    var warehouse = builder.Build();
    //    var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
    //    Assert.Contains(fact.Columns, e => e.Name == name);
    //}

    //[Fact]
    //public void CanChangeToValidTypeViaFluent()
    //{
    //    var builder = new WarehouseModelBuilder();
    //    builder.LoadSchema<MeasureContext>();

    //    builder.Fact<MeasureContainer>().Measure(e => e.Gross).HasColumnType(ColumnType.Double);

    //    var warehouse = builder.Build();
    //    var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
    //    Assert.Contains(fact.Columns, e => e.Name == "Gross" && e.ColumnType == ColumnType.Double);
    //}

    //[Fact]
    //public void CannotChangeToInvalidTypeViaFluent()
    //{
    //    var builder = new WarehouseModelBuilder();
    //    builder.LoadSchema<MeasureContext>();

    //    Assert.Throws<DryException>(() => builder.Fact<MeasureContainer>().Measure(e => e.Gross).HasColumnType(ColumnType.Text));
    //}

    //[Fact]
    //public void CannotHaveDuplicateMeasureNames()
    //{
    //    var builder = new WarehouseModelBuilder();
    //    builder.LoadSchema<MeasureContext>();

    //    Assert.Throws<DryException>(() => builder.Fact<MeasureContainer>().Measure(e => e.Gross).HasName("Big Bucks"));
    //}

    //[Fact]
    //public void FluentCanImplicitlyIgnoreMeasure()
    //{
    //    var builder = new WarehouseModelBuilder();
    //    builder.LoadSchema<MeasureContext>();

    //    builder.Fact<MeasureContainer>().Measure(e => e.AnnualRevenue).HasIgnore();
    //    var warehouse = builder.Build();

    //    var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
    //    Assert.DoesNotContain(fact.Columns, e => e.Name == "Annual Revenue");
    //}

    //[Fact]
    //public void FluentCanExplicitlyIgnoreMeasure()
    //{
    //    var builder = new WarehouseModelBuilder();
    //    builder.LoadSchema<MeasureContext>();

    //    builder.Fact<MeasureContainer>().Measure(e => e.AnnualRevenue).HasIgnore(true);
    //    var warehouse = builder.Build();

    //    var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
    //    Assert.DoesNotContain(fact.Columns, e => e.Name == "Annual Revenue");
    //}

    //[Fact]
    //public void FluentCanUnIgnoreMeasure()
    //{
    //    var builder = new WarehouseModelBuilder();
    //    builder.LoadSchema<MeasureContext>();

    //    builder.Fact<MeasureContainer>().Measure(e => e.Ignored).HasIgnore(false);
    //    var warehouse = builder.Build();

    //    var fact = warehouse.Facts.Single(e => e.EntityType == typeof(MeasureContainer));
    //    Assert.Contains(fact.Columns, e => e.Name == "Ignored");
    //}

    //[Fact]
    //public void DuplicateNamesByConventionException()
    //{
    //    var builder = new WarehouseModelBuilder();

    //    Assert.Throws<DryException>(() => builder.LoadSchema<NameCollisionContext>());
    //}

    //[Fact]
    //public void DuplicateNamesByFluentException()
    //{
    //    var builder = new WarehouseModelBuilder();
    //    builder.LoadSchema<MeasureContext>();

    //    Assert.Throws<DryException>(() => builder.Fact<MeasureContainer>().Measure(e => e.Short).HasName("Integer"));
    //}

}

public class AttributeContext : DbContext {

    public DbSet<AttributeContainer> Attributes { get; set; } = null!;
}

[DimensionTable]
public class AttributeContainer {

    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    [Measure]
    public string Title { get; set; } = string.Empty;

    public int Integer { get; set; }

    public short Short { get; set; }

    public float Float { get; set; }

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

//public class EmptyMeasureNameContext : DbContext {

//    [FactTable]
//    public class BadClass {

//        [Measure("")]
//        public decimal GoodName { get; set; }

//    }

//    public DbSet<BadClass> BadClasses { get; set; } = null!;
//}

//public class BlankMeasureNameContext : DbContext {

//    [FactTable]
//    public class BadClass {

//        [Measure("   ")]
//        public decimal GoodName { get; set; }

//    }

//    public DbSet<BadClass> BadClasses { get; set; } = null!;
//}

//public class NameCollisionContext : DbContext {

//    [FactTable]
//    public class NameCollisionClass {

//        public decimal Name { get; set; }

//        [Measure("Name")]
//        public decimal Title { get; set; }
//    }

//    public DbSet<NameCollisionClass> NameCollisionClasses { get; set; } = null!;
//}
