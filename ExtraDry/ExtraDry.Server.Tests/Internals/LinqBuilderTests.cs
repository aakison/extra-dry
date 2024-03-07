using ExtraDry.Server.Internal;
using System.Reflection;

namespace ExtraDry.Server.Tests.Internals;

public class LinqBuilderTests
{

    [Fact]
    public void OrderByNameCompatible()
    {
        var linqSorted = SampleData.OrderBy(e => e.FirstName).ToList();

        var linqBuilderSorted = SampleData.AsQueryable().OrderBy("FirstName").ToList();

        Assert.Equal(linqSorted, linqBuilderSorted);
    }

    [Fact]
    public void OrderByNumberCompatible()
    {
        var linqSorted = SampleData.OrderBy(e => e.Number).ToList();

        var linqBuilderSorted = SampleData.AsQueryable().OrderBy("Number").ToList();

        Assert.Equal(linqSorted, linqBuilderSorted);
    }

    [Fact]
    public void OrderByPublicNameCompatible()
    {
        var linqSorted = SampleData.OrderBy(e => e.InternalName).ToList();

        var linqBuilderSorted = SampleData.AsQueryable().OrderBy("PublicName").ToList();

        Assert.Equal(linqSorted, linqBuilderSorted);
    }

    [Fact]
    public void OrderByDescendingNameCompatible()
    {
        var linqSorted = SampleData.OrderByDescending(e => e.FirstName).ToList();

        var linqBuilderSorted = SampleData.AsQueryable().OrderByDescending("FirstName").ToList();

        Assert.Equal(linqSorted, linqBuilderSorted);
    }

    [Fact]
    public void OrderByDescendingNumberCompatible()
    {
        var linqSorted = SampleData.OrderByDescending(e => e.Number).ToList();

        var linqBuilderSorted = SampleData.AsQueryable().OrderByDescending("Number").ToList();

        Assert.Equal(linqSorted, linqBuilderSorted);
    }

    [Fact]
    public void OrderByInvalidNameException()
    {
        Assert.Throws<DryException>(() =>
            SampleData.AsQueryable().OrderByDescending("Invalid").ToList()
        );
    }

    [Fact]
    public void ThenByCompatible()
    {
        var linqSorted = SampleData.OrderBy(e => e.FirstName).ThenBy(e => e.Number).ToList();

        var linqBuilderSorted = SampleData.AsQueryable().OrderBy("FirstName").ThenBy("Number").ToList();

        Assert.Equal(linqSorted, linqBuilderSorted);
    }

    [Fact]
    public void ThenByDescendingCompatible()
    {
        var linqSorted = SampleData.OrderByDescending(e => e.FirstName).ThenByDescending(e => e.Number).ToList();

        var linqBuilderSorted = SampleData.AsQueryable().OrderByDescending("FirstName").ThenByDescending("Number").ToList();

        Assert.Equal(linqSorted, linqBuilderSorted);
    }

    [Fact]
    public void SingleEqualsWhereFilterCompatible()
    {
        var linqWhere = SampleData.Where(e => e.FirstName == "Bob").ToList();

        var filterProperty = GetFilterProperty("FirstName");
        var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "firstname:Bob").ToList();

        Assert.Equal(linqWhere, linqBuilderWhere);
    }

    [Fact]
    public void SingleEqualsWhereFilterJsonNameCompatible()
    {
        var linqWhere = SampleData.Where(e => e.InternalName == "Bobby").ToList();
        var modelDescription = new ModelDescription(typeof(Datum));

        var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(modelDescription.FilterProperties.ToArray(), "publicname:Bobby").ToList();

        Assert.Equal(linqWhere, linqBuilderWhere);
    }

    [Fact]
    public void SingleStartsWithWhereFilterCompatible()
    {
        var linqWhere = SampleData.Where(e => e.LastName.StartsWith("Bark", StringComparison.Ordinal)).ToList();

        var filterProperty = GetFilterProperty("LastName");
        var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "LastName:Bark").ToList();

        Assert.Equal(linqWhere, linqBuilderWhere);
    }

    [Fact]
    public void MultipleWhereFilterCompatible()
    {
        var linqWhere = SampleData.Where(e => e.FirstName == "Bob" || e.LastName.StartsWith("Bark", StringComparison.Ordinal)).ToList();

        var firstName = GetFilterProperty("FirstName");
        var lastName = GetFilterProperty("LastName");
        var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { firstName, lastName }, "firstname:Bob lastname:Bark").ToList();

        Assert.Equal(linqWhere, linqBuilderWhere);
    }

    [Theory]
    [MemberData(nameof(FilteredDatesData))]
    public void FilteredDates(List<Datum> sampleData, string filterProperty, string filterValue, Func<Datum, bool> expectedResults)
    {
        var linqWhere = sampleData.Where(expectedResults).ToList();

        var property = GetFilterProperty(filterProperty);
        var linqBuilderWhere = sampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { property }, $"{filterProperty}:{filterValue}").ToList();

        Assert.Equal(linqWhere, linqBuilderWhere);
    }

    public static IEnumerable<object[]> FilteredDatesData =>
        new List<object[]>
        {
            // 1. Test inclusive start year
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2023, 1, 1) },
                    new Datum { Created = new DateTime(2022, 12, 31) },
                    new Datum { Created = new DateTime(2024, 5, 23) },
                }, "Created", "[2023,]", new Func<Datum, bool>(e => e.Created >= new DateTime(2023, 1, 1)),
            },
            // 2. Test exclusive start year
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2024, 1, 1) },
                    new Datum { Created = new DateTime(2023, 12, 31) },
                    new Datum { Created = new DateTime(2024, 5, 23) },
                }, "Created", "(2023,]", new Func<Datum, bool>(e => e.Created >= new DateTime(2024, 1, 1)),
            },
            // 3. Test inclusive end year
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2023, 12, 31) },
                    new Datum { Created = new DateTime(2024, 1, 1) },
                    new Datum { Created = new DateTime(2022, 5, 23) },
                }, "Created", "[,2023]", new Func < Datum, bool > (e => e.Created <= new DateTime(2023, 12, 31)),
            },
            // 4. Test exclusive end year
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2022, 12, 31) },
                    new Datum { Created = new DateTime(2023, 1, 1) },
                    new Datum { Created = new DateTime(2022, 5, 23) },
                }, "Created", "[,2023)", new Func < Datum, bool > (e => e.Created <= new DateTime(2022, 12, 31)),
            },
            // 5. Test inclusive start and end year
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2022, 12, 31) },
                    new Datum { Created = new DateTime(2023, 1, 1) },
                    new Datum { Created = new DateTime(2023, 5, 23) },
                    new Datum { Created = new DateTime(2024, 12, 31) },
                    new Datum { Created = new DateTime(2025, 1, 1) },
                }, "Created", "[2023,2024]", new Func<Datum, bool>(e => e.Created >= new DateTime(2023, 1, 1) && e.Created <= new DateTime(2024, 12, 31)),
            },
            // 6. Test inclusive start and end year
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2023, 12, 31) },
                    new Datum { Created = new DateTime(2024, 1, 1) },
                    new Datum { Created = new DateTime(2024, 5, 23) },
                    new Datum { Created = new DateTime(2024, 12, 31) },
                    new Datum { Created = new DateTime(2025, 1, 1) },
                }, "Created", "(2023,2025)", new Func<Datum, bool>(e => e.Created >= new DateTime(2024, 1, 1) && e.Created <= new DateTime(2024, 12, 31)),
            },
            // 7. Test nullable DateTime property
            new object[] {
                new List<Datum>() {
                    new Datum { Updated = new DateTime(2023, 1, 1) },
                    new Datum { Updated = new DateTime(2022, 12, 31) },
                    new Datum { Updated = new DateTime(2024, 5, 23) },
                }, "Updated", "[2023,]", new Func<Datum, bool>(e => e.Updated >= new DateTime(2023, 1, 1)),
            },
            // 8. Test inclusive start year and month
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2024, 2, 29) },
                    new Datum { Created = new DateTime(2024, 3, 1) },
                    new Datum { Created = new DateTime(2024, 5, 23) },
                }, "Created", "[2024-3,]", new Func<Datum, bool>(e => e.Created >= new DateTime(2024, 3, 1)),
            },
            // 9. Test exclusive start year and month
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2024, 3, 31) },
                    new Datum { Created = new DateTime(2024, 4, 1) },
                    new Datum { Created = new DateTime(2024, 5, 23) },
                }, "Created", "(2024-3,]", new Func<Datum, bool>(e => e.Created >= new DateTime(2024, 4, 1)),
            },
            // 10. Test inclusive end year and month
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2024, 4, 1) },
                    new Datum { Created = new DateTime(2024, 3, 31) },
                    new Datum { Created = new DateTime(2024, 2, 23) },
                }, "Created", "[,2024-3]", new Func<Datum, bool>(e => e.Created <= new DateTime(2024, 3, 31)),
            },
            // 11. Test inclusive end year and month with 30 days
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2024, 5, 1) },
                    new Datum { Created = new DateTime(2024, 4, 30) },
                    new Datum { Created = new DateTime(2024, 2, 23) },
                }, "Created", "[,2024-4]", new Func<Datum, bool>(e => e.Created <= new DateTime(2024, 4, 30)),
            },
            // 12. Test exclusive end year and month
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2024, 3, 1) },
                    new Datum { Created = new DateTime(2024, 2, 29) },
                    new Datum { Created = new DateTime(2024, 1, 23) },
                }, "Created", "[,2024-3)", new Func<Datum, bool>(e => e.Created <= new DateTime(2024, 2, 29)),
            },
            // 13. Test inclusive start and end month
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2024, 1, 31) },
                    new Datum { Created = new DateTime(2024, 2, 1) },
                    new Datum { Created = new DateTime(2023, 3, 29) },
                    new Datum { Created = new DateTime(2024, 4, 30) },
                    new Datum { Created = new DateTime(2025, 5, 1) },
                }, "Created", "[2024-2,2024-4]", new Func<Datum, bool>(e => e.Created >= new DateTime(2024, 2, 1) && e.Created <= new DateTime(2024, 4, 30)),
            },
            // 14. Test exclusive start and end month
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2024, 2, 29) },
                    new Datum { Created = new DateTime(2024, 3, 1) },
                    new Datum { Created = new DateTime(2023, 3, 24) },
                    new Datum { Created = new DateTime(2024, 3, 31) },
                    new Datum { Created = new DateTime(2025, 4, 1) },
                }, "Created", "(2024-2,2024-4)", new Func<Datum, bool>(e => e.Created >= new DateTime(2024, 3, 1) && e.Created <= new DateTime(2024, 3, 31)),
            },
            // 15. Test inclusive start day
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2024, 2, 27) },
                    new Datum { Created = new DateTime(2024, 2, 28) },
                    new Datum { Created = new DateTime(2024, 3, 29) },
                }, "Created", "[2024-2-28,]", new Func<Datum, bool>(e => e.Created >= new DateTime(2024, 2, 28)),
            },
            // 16. Test exclusive start day
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2024, 2, 28) },
                    new Datum { Created = new DateTime(2024, 2, 29) },
                    new Datum { Created = new DateTime(2024, 3, 29) },
                }, "Created", "(2024-2-28,]", new Func<Datum, bool>(e => e.Created >= new DateTime(2024, 2, 29)),
            },
            // 17. Test inclusive end day
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2024, 2, 29) },
                    new Datum { Created = new DateTime(2024, 2, 28) },
                    new Datum { Created = new DateTime(2024, 1, 29) },
                }, "Created", "[,2024-2-28]", new Func<Datum, bool>(e => e.Created <= new DateTime(2024, 2, 28)),
            },
            // 18. Test exclusive start day
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2024, 2, 28) },
                    new Datum { Created = new DateTime(2024, 2, 27) },
                    new Datum { Created = new DateTime(2024, 1, 29) },
                }, "Created", "[,2024-2-28)", new Func<Datum, bool>(e => e.Created <= new DateTime(2024, 2, 27)),
            },
            // 19. Test inclusive start and end day
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2024, 2, 27) },
                    new Datum { Created = new DateTime(2024, 2, 28) },
                    new Datum { Created = new DateTime(2023, 3, 2) },
                    new Datum { Created = new DateTime(2024, 3, 5) },
                    new Datum { Created = new DateTime(2025, 3, 6) },
                }, "Created", "[2024-2-28,2024-3-5]", new Func<Datum, bool>(e => e.Created >= new DateTime(2024, 2, 28) && e.Created <= new DateTime(2024, 3, 5)),
            },
            // 20. Test exclusive start and end month
            new object[] {
                new List<Datum>() {
                    new Datum { Created = new DateTime(2024, 2, 28) },
                    new Datum { Created = new DateTime(2024, 2, 29) },
                    new Datum { Created = new DateTime(2023, 3, 2) },
                    new Datum { Created = new DateTime(2024, 3, 4) },
                    new Datum { Created = new DateTime(2025, 3, 5) },
                }, "Created", "(2024-2-28,2024-3-5)", new Func<Datum, bool>(e => e.Created >= new DateTime(2024, 2, 29) && e.Created <= new DateTime(2024, 3, 4)),
            },
        };

    [Theory]
    [InlineData("Created", "[,]")]
    [InlineData("Created", "{2023,]")]
    [InlineData("Created", "[2023,}")]
    [InlineData("Created", "[2023:]")]
    [InlineData("Created", "[0000,]")]
    [InlineData("Created", "[20023,]")]
    [InlineData("Created", "[2023/1,]")]
    [InlineData("Created", "[2023-0,]")]
    [InlineData("Created", "[2023-13,]")]
    [InlineData("Created", "[13-2023,]")]
    [InlineData("Created", "[2023/1/1,]")]
    [InlineData("Created", "[2023-2-29,]")]
    [InlineData("Created", "[2024-2-30,]")]
    [InlineData("Created", "[2023-4-31,]")]
    [InlineData("Created", "[2023-3-32,]")]
    public void InvalidFilterDates(string filterProperty, string filterValue)
    {
        var property = GetFilterProperty(filterProperty);
        Assert.Throws<DryException>(() => SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { property }, $"{filterProperty}:{filterValue}"));
    }

    private static FilterProperty GetFilterProperty(string propertyName)
    {
        var property = typeof(Datum).GetProperty(propertyName) ?? throw new ArgumentException("Bad argument", nameof(propertyName));
        var filter = property.GetCustomAttribute<FilterAttribute>() ?? throw new ArgumentException("Bad argument", nameof(propertyName));
        return new FilterProperty(property, filter);
    }

    public class Datum
    {

        [JsonIgnore]
        [Key]
        public int Id { get; set; }

        [Filter(FilterType.Equals)]
        public string FirstName { get; set; } = string.Empty;

        [Filter(FilterType.StartsWith)]
        public string LastName { get; set; } = string.Empty;

        [Filter] //(FilterType.Contains)] TODO: Implement contains with full text index.
        public string Keywords { get; set; } = string.Empty;

        public int Number { get; set; }

        [Filter(FilterType.Equals)]
        [JsonPropertyName("publicName")]
        public string InternalName { get; set; } = string.Empty;

        [Filter]
        public DateTime Created { get; set; } = DateTime.Now;

        [Filter]
        public DateTime? Updated { get; set; }
    }

    private readonly List<Datum> SampleData = new() {
        new Datum { FirstName = "Charlie", LastName = "Coase", Number = 111, InternalName = "Chuck" },
        new Datum { FirstName = "Alice", LastName = "Cooper", Number = 333, InternalName = "Al" },
        new Datum { FirstName = "Bob", LastName = "Barker", Number = 222, InternalName = "Bobby" },
    };

    //private readonly List<Datum> SampleDataWithDuplicateNames = new() {
    //    new Datum { FirstName = "Charlie", LastName = "Coase", Number = 111},
    //    new Datum { FirstName = "Alice", LastName = "Cooper", Number = 333 },
    //    new Datum { FirstName = "Bob", LastName = "Barker", Number = 222 },
    //    new Datum { FirstName = "Alice", LastName = "Barker", Number = 123 },
    //    new Datum { FirstName = "Bob", LastName = "Ross", Number = 321 },
    //};

}
