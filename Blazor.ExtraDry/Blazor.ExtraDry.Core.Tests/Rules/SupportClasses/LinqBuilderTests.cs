using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Blazor.ExtraDry;

namespace Blazor.ExtraDry.Core.Tests.Rules.SupportClasses {
    public class LinqBuilderTests {

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
            var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, new string[] { "Bob" }).ToList();

            Assert.Equal(linqWhere, linqBuilderWhere);
        }

        [Fact]
        public void SingleStartsWithWhereFilterCompatible()
        {
            var linqWhere = SampleData.Where(e => e.LastName.StartsWith("Bark")).ToList();

            var filterProperty = GetFilterProperty("LastName");
            var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, new string[] { "Bark" }).ToList();

            Assert.Equal(linqWhere, linqBuilderWhere);
        }

        [Fact]
        public void MultipleWhereFilterCompatible()
        {
            var linqWhere = SampleData.Where(e => e.FirstName == "Bob" || e.LastName.StartsWith("Bark")).ToList();

            var firstName = GetFilterProperty("FirstName");
            var lastName = GetFilterProperty("LastName");
            var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { firstName, lastName }, new string[] { "Bob", "Bark" }).ToList();

            Assert.Equal(linqWhere, linqBuilderWhere);
        }

        private static FilterProperty GetFilterProperty(string propertyName)
        {
            var property = typeof(Datum).GetProperty(propertyName);
            var filter = property.GetCustomAttributes(false).First() as FilterAttribute;
            return new FilterProperty(property, filter);
        }

        public class Datum {
            [Filter(FilterType.Equals)]
            public string FirstName { get; set; }

            [Filter(FilterType.StartsWith)]
            public string LastName { get; set; }

            [Filter(FilterType.Contains)]
            public string Keywords { get; set; }

            public int Number { get; set; }
        }

        private List<Datum> SampleData = new List<Datum>() {
            new Datum { FirstName = "Charlie", LastName = "Coase", Number = 111},
            new Datum { FirstName = "Alice", LastName = "Cooper", Number = 333 },
            new Datum { FirstName = "Bob", LastName = "Barker", Number = 222 },
        };

        private List<Datum> SampleDataWithDuplicateNames = new List<Datum>() {
            new Datum { FirstName = "Charlie", LastName = "Coase", Number = 111},
            new Datum { FirstName = "Alice", LastName = "Cooper", Number = 333 },
            new Datum { FirstName = "Bob", LastName = "Barker", Number = 222 },
            new Datum { FirstName = "Alice", LastName = "Barker", Number = 123 },
            new Datum { FirstName = "Bob", LastName = "Ross", Number = 321 },
        };

    }
}
