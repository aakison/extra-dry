#nullable enable

using Xunit;

namespace ExtraDry.Core.Tests.Rules {
    public class FilterAttributeTests {

        [Fact]
        public void DefaultValues()
        {
            var filter = ValidFilter;

            Assert.Equal(FilterType.Equals, filter.Type);
        }

        [Theory]
        [InlineData("Type", FilterType.StartsWith)]
        public void RoundtripProperties(string propertyName, object propertyValue)
        {
            var filter = ValidFilter;
            var property = filter.GetType().GetProperty(propertyName);

            property?.SetValue(filter, propertyValue);
            var result = property?.GetValue(filter);

            Assert.Equal(propertyValue, result);
        }

        private readonly FilterAttribute ValidFilter = new();
    }
}
