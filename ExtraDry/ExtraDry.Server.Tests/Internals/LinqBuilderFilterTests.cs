using ExtraDry.Server.Internal;
using System.Reflection;

namespace ExtraDry.Server.Tests.Internals;

public class LinqBuilderFilterTests {

    [Fact]
    public void SingleEqualsWhereFilterCompatible()
    {
        var linqWhere = SampleData.Where(e => e.FirstName == "Bob").ToList();
        var filterProperty = GetFilterProperty<Datum>(nameof(Datum.FirstName));

        var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "firstname:Bob").ToList();

        Assert.Equal(linqWhere, linqBuilderWhere);
    }

    [Fact]
    public void MultipleNamesOnSingleField()
    {
        var linqWhere = SampleData.Where(e => e.FirstName == "Bob" || e.FirstName == "Alice").ToList();
        var filterProperty = GetFilterProperty<Datum>(nameof(Datum.FirstName));

        var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "firstname:Bob|Alice").ToList();

        Assert.Equal(linqWhere, linqBuilderWhere);
    }

    [Fact]
    public void MultipleNamesOnSingleFieldAlternateSyntax()
    {
        var linqWhere = SampleData.Where(e => e.FirstName == "Bob" || e.FirstName == "Alice").ToList();
        var filterProperty = GetFilterProperty<Datum>(nameof(Datum.FirstName));

        var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "firstname:Bob firstname:Alice").ToList();

        Assert.Equal(linqWhere, linqBuilderWhere);
    }

    [Fact]
    public void ContainsFilterAttribute()
    {
        var linqWhere = SampleData.Where(e => e.Keywords.Contains("beta")).ToList();
        var filterProperty = GetFilterProperty<Datum>(nameof(Datum.Keywords));

        var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "keywords:beta").ToList();

        Assert.Equal(linqWhere, linqBuilderWhere);
    }

    [Fact]
    public void FilterOnNumberField()
    {
        var linqWhere = SampleData.Where(e => e.Number == 222).ToList();
        var filterProperty = GetFilterProperty<Datum>(nameof(Datum.Number));

        var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "number:222").ToList();

        Assert.Equal(linqWhere, linqBuilderWhere);
    }

    [Fact]
    public void FilterOnMultipleNumberField()
    {
        var linqWhere = SampleData.Where(e => e.Number == 222 || e.Number == 111).ToList();
        var filterProperty = GetFilterProperty<Datum>(nameof(Datum.Number));

        var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "number:222|111").ToList();

        Assert.Equal(linqWhere, linqBuilderWhere);
    }

    [Fact]
    public void SimpleRangeQuery()
    {
        var linqWhere = SampleDataWithDuplicateNames.Where(e => e.Number >= 100 && e.Number < 200).ToList();
        var firstName = GetFilterProperty<Datum>(nameof(Datum.FirstName));
        var lastName = GetFilterProperty<Datum>(nameof(Datum.LastName));
        var number = GetFilterProperty<Datum>(nameof(Datum.Number));

        var linqBuilderWhere = SampleDataWithDuplicateNames.AsQueryable().WhereFilterConditions(new FilterProperty[] { firstName, lastName, number }, "number:[100,200)").ToList();

        Assert.Equal(linqWhere, linqBuilderWhere);
    }

    [Theory]
    [InlineData("number:[100,200)")]
    [InlineData("number:[123,223)")]
    [InlineData("number:[123,222]")]
    [InlineData("number:(122,223)")]
    [InlineData("number:(122,222]")]
    [InlineData("firstname:Bob")] // Ignores case on property (test values, however, are case sensitive...)
    [InlineData("FIRSTNAME:Bob")]
    [InlineData("lastname:Co")] // startswith condition on Filter on property
    public void QueriesThatPickTwo(string filter)
    {
        var firstName = GetFilterProperty<Datum>(nameof(Datum.FirstName));
        var lastName = GetFilterProperty<Datum>(nameof(Datum.LastName));
        var number = GetFilterProperty<Datum>(nameof(Datum.Number));

        var linqBuilderWhere = SampleDataWithDuplicateNames.AsQueryable().WhereFilterConditions(new FilterProperty[] { firstName, lastName, number }, filter).ToList();

        Assert.Equal(2, linqBuilderWhere.Count);
    }

    [Theory]
    [InlineData("active", 2)]
    [InlineData("inactive", 1)]
    [InlineData("status:active", 2)]
    [InlineData("status:inactive", 1)]
    [InlineData("firstname:active", 0)] // don't match on status
    [InlineData("status:act", 0)] // don't match on status prefix
    public void QueryThatPicksEnum(string filter, int count)
    {
        var status = GetFilterProperty<Datum>(nameof(Datum.Status));
        var firstName = GetFilterProperty<Datum>(nameof(Datum.FirstName));

        var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { status, firstName }, filter).ToList();

        Assert.Equal(count, linqBuilderWhere.Count);
    }

    [Fact]
    public void StringRangeNotSupported()
    {
        var firstName = GetFilterProperty<Datum>(nameof(Datum.FirstName));
        var lastName = GetFilterProperty<Datum>(nameof(Datum.LastName));
        var number = GetFilterProperty<Datum>(nameof(Datum.Number));

        Assert.Throws<DryException>(() => SampleDataWithDuplicateNames.AsQueryable().WhereFilterConditions(new FilterProperty[] { firstName, lastName, number }, "lastname:[coa,coo]").ToList());
    }

    [Fact]
    public void WildcardSearchWhenOnlySingleFilterField()
    {
        var SampleData = new List<SimpleDatum>() {
            new() { Name = "Charlie" },
            new() { Name = "Alice" },
        };
        var name = GetFilterProperty<SimpleDatum>(nameof(SimpleDatum.Name));
        var filter = "Alice";

        var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { name }, filter).ToList();

        Assert.Single(linqBuilderWhere);
        Assert.Equal("Alice", linqBuilderWhere.First().Name);
    }

    private static FilterProperty GetFilterProperty<T>(string propertyName)
    {
        var property = typeof(T).GetProperty(propertyName) ?? throw new ArgumentException("Invalid argument", nameof(propertyName));
        var filter = property.GetCustomAttribute<FilterAttribute>() ?? throw new ArgumentException("Invalid argument", nameof(propertyName));
        return new FilterProperty(property, filter);
    }

    public class Datum {
        [Filter(FilterType.Equals)]
        public string FirstName { get; set; } = string.Empty;

        [Filter(FilterType.StartsWith)]
        public string LastName { get; set; } = string.Empty;

        [Filter(FilterType.Contains)]
        public string Keywords { get; set; } = string.Empty;

        [Filter]
        public int Number { get; set; }

        [Filter]
        public DatumType Status { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DatumType
    {
        Active,
        Inactive,
    }

    private readonly List<Datum> SampleData = [
        new Datum { FirstName = "Charlie", LastName = "Coase", Number = 111, Keywords = "alpha beta gamma", Status = DatumType.Active },
        new Datum { FirstName = "Alice", LastName = "Cooper", Number = 333, Keywords = "beta gamma delta", Status = DatumType.Active  },
        new Datum { FirstName = "Bob", LastName = "Barker", Number = 222, Keywords = "gamma delta epsilon", Status = DatumType.Inactive  },
    ];

    private readonly List<Datum> SampleDataWithDuplicateNames = [
        new Datum { FirstName = "Charlie", LastName = "Coase", Number = 111},
        new Datum { FirstName = "Alice", LastName = "Cooper", Number = 333 },
        new Datum { FirstName = "Bob", LastName = "Barker", Number = 222 },
        new Datum { FirstName = "Alice", LastName = "Barker", Number = 123 },
        new Datum { FirstName = "Bob", LastName = "Ross", Number = 321 },
    ];

    public class SimpleDatum {
        [Filter(FilterType.Equals)]
        public string Name { get; set; } = string.Empty;
    }

}
