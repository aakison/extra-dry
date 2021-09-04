#nullable enable

using ExtraDry.Core;
using Xunit;

namespace ExtraDry.Core.Tests.Internals {

    public class FilterQueryTests {

        [Fact]
        public void DefaultValues()
        {
            var filterQuery = ValidFilterQuery;

            Assert.False(filterQuery.Ascending);
            Assert.Null(filterQuery.Filter);
            Assert.Null(filterQuery.Sort);
            Assert.Null(filterQuery.Stabilizer);
        }

        [Theory]
        [InlineData("Ascending", true)]
        [InlineData("Ascending", false)]
        [InlineData("Filter", null)]
        [InlineData("Filter", "not")]
        [InlineData("Sort", null)]
        [InlineData("Sort", "not")]
        [InlineData("Stabilizer", null)]
        [InlineData("Stabilizer", "not")]
        public void RoundtripProperties(string propertyName, object propertyValue)
        {
            var filter = ValidFilterQuery;
            var property = filter.GetType().GetProperty(propertyName);

            property?.SetValue(filter, propertyValue);
            var result = property?.GetValue(filter);

            Assert.Equal(propertyValue, result);
        }

        private FilterQuery ValidFilterQuery => new FilterQuery();

    }
}
