#nullable enable

using Xunit;

namespace Blazor.ExtraDry.Core.Tests.Rules {
    public class FilterAttributeTests {

        [Fact]
        public void DefaultValues()
        {
            var filter = ValidFilter;

            Assert.Equal(FilterType.Contains, filter.Type);
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

        private static FilterAttribute ValidFilter = new();
    }
}
