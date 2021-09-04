using ExtraDry.Server.Internal;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExtraDry.Core.Tests.Internals {

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

        [Fact]
        public void ContainsFilterAttribute()
        {
            var linqWhere = SampleData.Where(e => e.Keywords.Contains("beta")).ToList();

            var filterProperty = GetFilterProperty("Keywords");
            var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "keywords:beta").ToList();

            Assert.Equal(linqWhere, linqBuilderWhere);
        }

        [Fact]
        public void FilterOnNumberField()
        {
            var linqWhere = SampleData.Where(e => e.Number == 222).ToList();

            var filterProperty = GetFilterProperty("Number");
            var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "number:222").ToList();

            Assert.Equal(linqWhere, linqBuilderWhere);
        }

        [Fact]
        public void FilterOnMultipleNumberField()
        {
            var linqWhere = SampleData.Where(e => e.Number == 222 || e.Number == 111).ToList();

            var filterProperty = GetFilterProperty("Number");
            var linqBuilderWhere = SampleData.AsQueryable().WhereFilterConditions(new FilterProperty[] { filterProperty }, "number:222|111").ToList();

            Assert.Equal(linqWhere, linqBuilderWhere);
        }

        [Fact]
        public void SimpleRangeQuery()
        {
            var linqWhere = SampleDataWithDuplicateNames.Where(e => e.Number >= 100 && e.Number < 200).ToList();

            var firstName = GetFilterProperty("FirstName");
            var lastName = GetFilterProperty("LastName");
            var number = GetFilterProperty("Number");
            var linqBuilderWhere = SampleDataWithDuplicateNames.AsQueryable().WhereFilterConditions(new FilterProperty[] { firstName, lastName, number }, "number:[100,200)").ToList();

            Assert.Equal(linqWhere, linqBuilderWhere);
        }

        [Theory]
        [InlineData("number:[100,200)")]
        [InlineData("number:[123,223)")]
        [InlineData("number:[123,222]")]
        [InlineData("number:(122,223)")]
        [InlineData("number:(122,222]")]
        [InlineData("firstname:bob")]
        [InlineData("FIRSTNAME:BOB")]
        [InlineData("lastname:co")] // startswith condition on Filter on property
        public void QueriesThatPickTwo(string filter)
        {
            var firstName = GetFilterProperty("FirstName");
            var lastName = GetFilterProperty("LastName");
            var number = GetFilterProperty("Number");
            var linqBuilderWhere = SampleDataWithDuplicateNames.AsQueryable().WhereFilterConditions(new FilterProperty[] { firstName, lastName, number }, filter).ToList();

            Assert.Equal(2, linqBuilderWhere.Count);
        }

        [Fact]
        public void StringRangeNotSupported()
        {
            var firstName = GetFilterProperty("FirstName");
            var lastName = GetFilterProperty("LastName");
            var number = GetFilterProperty("Number");

            Assert.Throws<DryException>(() => SampleDataWithDuplicateNames.AsQueryable().WhereFilterConditions(new FilterProperty[] { firstName, lastName, number }, "lastname:[coa,coo]").ToList());
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

            [Filter]
            public int Number { get; set; }
        }

        private List<Datum> SampleData = new List<Datum>() {
            new Datum { FirstName = "Charlie", LastName = "Coase", Number = 111, Keywords = "alpha beta gamma"},
            new Datum { FirstName = "Alice", LastName = "Cooper", Number = 333, Keywords = "beta gamma delta" },
            new Datum { FirstName = "Bob", LastName = "Barker", Number = 222, Keywords = "gamma delta epsilon" },
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
