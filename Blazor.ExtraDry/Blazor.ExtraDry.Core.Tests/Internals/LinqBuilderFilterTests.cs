using Blazor.ExtraDry;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Blazor.ExtraDry.Core.Tests.Internals {

    public class LinqBuilderFilterTests {

        [Fact]
        public void SingleEqualsWhereFilterCompatible()
        {
            var linqWhere = SampleData.Where(e => e.FirstName == "Bob").ToList();

            var filterProperty = GetFilterProperty("FirstName");
            var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "firstname:bob").ToList();

            Assert.Equal(linqWhere, linqBuilderWhere);
        }

        [Fact]
        public void MissingFilterAttributeOnProperty()
        {
            var linqWhere = SampleData.Where(e => e.FirstName == "Bob").ToList();

            var filterProperty = GetFilterProperty("FirstName");
            Assert.Throws<DryException>(() => SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "number:bob"));
        }

        [Fact]
        public void MultipleNamesOnSingleField()
        {
            var linqWhere = SampleData.Where(e => e.FirstName == "Bob" || e.FirstName == "Alice").ToList();

            var filterProperty = GetFilterProperty("FirstName");
            var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "firstname:bob|alice").ToList();

            Assert.Equal(linqWhere, linqBuilderWhere);
        }

        [Fact]
        public void MultipleNamesOnSingleFieldAlternateSyntax()
        {
            var linqWhere = SampleData.Where(e => e.FirstName == "Bob" || e.FirstName == "Alice").ToList();

            var filterProperty = GetFilterProperty("FirstName");
            var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "firstname:bob firstname:alice").ToList();

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
