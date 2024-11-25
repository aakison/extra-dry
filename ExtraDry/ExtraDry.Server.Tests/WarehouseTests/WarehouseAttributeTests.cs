using ExtraDry.Core.DataWarehouse;
using ExtraDry.Server.DataWarehouse;

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
    public void AttributeIncluded(string name, ColumnType columnType)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<AttributeContext>();
        var warehouse = builder.Build();

        var dimension = warehouse.Dimensions.Single(e => e.EntityType == typeof(AttributeContainer));
        Assert.Contains(dimension.Columns, e => e.Name == name && e.ColumnType == columnType);
    }

    [Theory]
    [InlineData("ID")] // Key property doesn't slip through as an attribute.
    //[InlineData("Integer")] // int column not an attribute; but can be such as 'SortOrder', removed (scar tissue to remind us).
    [InlineData("Double")] // double String column not an attribute.
    [InlineData("Decimal")] // decimal column not an attribute.
    [InlineData("Correlation")] // EF NotMappedAttribue suppresses by default.
    [InlineData("Ignored")] // MeasureIgnoreAttribue suppresses.
    public void AttributeNotIncluded(string name)
    {
        var builder = new WarehouseModelBuilder();

        builder.LoadSchema<AttributeContext>();
        var warehouse = builder.Build();

        var dimension = warehouse.Dimensions.Single(e => e.EntityType == typeof(AttributeContainer));
        Assert.DoesNotContain(dimension.Columns, e => e.Name == name);
    }

    [Fact]
    public void FluentRenameOfMeasure()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<AttributeContext>();

        builder.Dimension<AttributeContainer>().Attribute(e => e.LastName).HasName("Surname");

        var warehouse = builder.Build();
        var dimension = warehouse.Dimensions.Single(e => e.EntityType == typeof(AttributeContainer));
        Assert.Contains(dimension.Columns, e => e.Name == "Surname" && e.ColumnType == ColumnType.Text);
    }

    [Theory]
    [InlineData(typeof(EmptyAttributeNameContext))]
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
    public void BadAttributeNameInFluent(string name)
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<AttributeContext>();

        Assert.Throws<DryException>(() => builder.Dimension<AttributeContainer>().Attribute(e => e.LastName).HasName(name));
    }

    [Theory]
    [InlineData("012345678901234567890123456789012345678901234567")]
    [InlineData("!!!")]
    [InlineData("!@#$")]
    public void WackyButValidMeasureNameInFluent(string name)
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<AttributeContext>();

        builder.Dimension<AttributeContainer>().Attribute(e => e.LastName).HasName(name);

        var warehouse = builder.Build();
        var dimension = warehouse.Dimensions.Single(e => e.EntityType == typeof(AttributeContainer));
        Assert.Contains(dimension.Columns, e => e.Name == name);
    }

    [Fact]
    public void CannotHaveDuplicateAttributeNames()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<AttributeContext>();

        Assert.Throws<DryException>(() => builder.Dimension<AttributeContainer>().Attribute(e => e.LastName).HasName("Caption"));
    }

    [Fact]
    public void FluentCanExplicitlyIgnoreAttribute()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<AttributeContext>();

        builder.Dimension<AttributeContainer>().Attribute(e => e.Name).IsIncluded(false);
        var warehouse = builder.Build();

        var dimension = warehouse.Dimensions.Single(e => e.EntityType == typeof(AttributeContainer));
        Assert.DoesNotContain(dimension.Columns, e => e.Name == "Name");
    }

    [Fact]
    public void FluentCanUnIgnoreMeasure()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<AttributeContext>();

        builder.Dimension<AttributeContainer>().Attribute(e => e.Ignored).IsIncluded(true);
        var warehouse = builder.Build();

        var dimension = warehouse.Dimensions.Single(e => e.EntityType == typeof(AttributeContainer));
        Assert.Contains(dimension.Columns, e => e.Name == "Ignored");
    }

    [Fact]
    public void DuplicateNamesByConventionException()
    {
        var builder = new WarehouseModelBuilder();

        Assert.Throws<DryException>(() => builder.LoadSchema<AttributeNameCollisionContext>());
    }

    [Fact]
    public void DuplicateNamesByFluentException()
    {
        var builder = new WarehouseModelBuilder();
        builder.LoadSchema<AttributeContext>();

        Assert.Throws<DryException>(() => builder.Dimension<AttributeContainer>().Attribute(e => e.LastName).HasName("Name"));
    }

    public class AttributeContext : DbContext {
        public DbSet<AttributeContainer> Attributes { get; set; } = null!;
    }

    [DimensionTable]
    public class AttributeContainer {

        [Key]
        [JsonIgnore]
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

        public int WholeNumberTest { get; set; }

        public double RealNumberTest { get; set; }

        public decimal CurrencyNumberTest { get; set; }
    }


}


public class EmptyAttributeNameContext : DbContext {

    [DimensionTable]
    public class BadClass {

        [Measure("")]
        public string GoodName { get; set; } = string.Empty;

    }

    public DbSet<BadClass> BadClasses { get; set; } = null!;
}

public class BlankAttributeNameContext : DbContext {

    [DimensionTable]
    public class BadClass {

        [Measure("   ")]
        public string GoodName { get; set; } = string.Empty;

    }

    public DbSet<BadClass> BadClasses { get; set; } = null!;
}

public class AttributeNameCollisionContext : DbContext {

    [DimensionTable]
    public class NameCollisionClass {

        public string Name { get; set; } = string.Empty;

        [Attribute("Name")]
        public string Title { get; set; } = string.Empty;
    }

    public DbSet<NameCollisionClass> NameCollisionClasses { get; set; } = null!;
}
