#nullable enable

using ExtraDry.Core;
using Xunit;

namespace ExtraDry.Core.Tests.Rules {

    public class HeaderAttributeTests {

        [Fact]
        public void DefaultValues()
        {
            var header = ValidHeaderAttribute;

            Assert.Equal(ColumnType.Primary, header.Column);
            Assert.Null(header.Description);
            Assert.Equal("title", header.Title);
        }

        [Theory]
        [InlineData("Title", "another")]
        [InlineData("Description", null)]
        [InlineData("Description", "abc")]
        [InlineData("Column", ColumnType.Alternate)]
        public void RoundtripProperties(string propertyName, object propertyValue)
        {
            var filter = ValidHeaderAttribute;
            var property = filter.GetType().GetProperty(propertyName);

            property?.SetValue(filter, propertyValue);
            var result = property?.GetValue(filter);

            Assert.Equal(propertyValue, result);
        }

        private HeaderAttribute ValidHeaderAttribute => new HeaderAttribute("title");

    }
}
