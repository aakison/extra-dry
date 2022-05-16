using ExtraDry.Core.DataWarehouse;
using ExtraDry.Server.DataWarehouse;
using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Server.Tests.WarehouseTests;

public class WarehouseAttributeTests {

    [Theory]
    [InlineData("Attribute Container ID", ColumnType.Key)] // Key Column naming convention
    [InlineData("Uuid", ColumnType.Text)] // Simple Guid property, no decoration or special handling
    [InlineData("Name", ColumnType.Text)] // Simple string property, no decoration or special handling
    [InlineData("Uri", ColumnType.Text)] // Simple Uri property, no decoration or special handling
    [InlineData("Caption", ColumnType.Text)] // Simple string property, with naming conversion
    [InlineData("Christian Name", ColumnType.Text)] // Decimal property with getter only, with naming conversion
    [InlineData("Notes", ColumnType.Text)] // NotMapped, but also Attribute so include it.
    [InlineData("First Name", ColumnType.Text)] // Convention attribute rename
    public void MeasureIncluded(string name, ColumnType columnType)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<AttributeContext>();
        var warehouse = builder.Build();

        var fact = warehouse.Dimensions.Single(e => e.EntityType == typeof(AttributeContainer));
        Assert.Contains(fact.Columns, e => e.Name == name && e.ColumnType == columnType);
    }

    [Theory]
    [InlineData("ID")] // Key property doesn't slip through as an attribute.
    [InlineData("Integer")] // int column not an attribute.
    [InlineData("Double")] // double String column not an attribute.
    [InlineData("Decimal")] // decimal column not an attribute.
    [InlineData("Correlation")] // EF NotMappedAttribue suppresses by default.
    [InlineData("Ignored")] // MeasureIgnoreAttribue suppresses.
    public void MeasureNotIncluded(string name)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<AttributeContext>();
        var warehouse = builder.Build();

        var fact = warehouse.Dimensions.Single(e => e.EntityType == typeof(AttributeContainer));
        Assert.DoesNotContain(fact.Columns, e => e.Name == name);
    }

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

    public Guid Uuid { get; set; }

    public string Name { get; set; } = string.Empty;

    public Uri Uri { get; set; } = null!;

    [Attribute("Caption")]
    public string Title { get; set; } = string.Empty;

    [NotMapped]
    public string Correlation { get; set; } = string.Empty;

    [NotMapped, Attribute]
    public string Notes { get; set; } = string.Empty;

    // For title casing
    public string FirstName { get; set; } = string.Empty;

    // For Fluent renaming
    public string LastName { get; set; } = string.Empty;

    public string ChristianName { 
        get => FirstName;
    }

    [AttributeIgnore]
    public string Ignored { get; set; } = string.Empty;

    public int Integer { get; set; }

    public double Double { get; set; }  

    public decimal Decimal { get; set; }
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
