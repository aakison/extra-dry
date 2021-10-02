using Xunit;

namespace ExtraDry.Core.Tests {

    public class DecimalRangeAttributeTests {

        [Theory]
        [InlineData("Minimum", 1)]
        [InlineData("Maximum", 2)]
        public void RoundtripDecimalProperty(string propertyName, decimal propertyValue)
        {
            var decimalRange = new DecimalRangeAttribute(0, 0);
            var property = decimalRange.GetType().GetProperty(propertyName);

            property.SetValue(decimalRange, propertyValue);
            var result = property.GetValue(decimalRange);

            Assert.Equal(propertyValue, result);
        }

        [Theory]
        [InlineData("ErrorMessage", "X")]
        public void RoundtripProperties(string propertyName, object propertyValue)
        {
            var decimalRange = new DecimalRangeAttribute(0, 0);
            var property = decimalRange.GetType().GetProperty(propertyName);

            property.SetValue(decimalRange, propertyValue);
            var result = property.GetValue(decimalRange);

            Assert.Equal(propertyValue, result);
        }

        [Theory]
        [InlineData(2, 1, 3)]
        [InlineData(0, -1, 1)]
        [InlineData(-2, -3, -1)]
        public void ValidValues(decimal value, decimal min, decimal max)
        {
            var range = new DecimalRangeAttribute((double)min, (double)max);

            var valid = range.IsValid(value);

            Assert.True(valid);
        }

        [Theory]
        [InlineData(4, 1, 3)]
        [InlineData(0, 1, 3)]
        [InlineData(2, -1, 1)]
        [InlineData(-2, -1, 1)]
        [InlineData(-4, -3, -1)]
        [InlineData(0, -3, -1)]
        [InlineData(0, 3, -3)] // min > max
        public void InvalidValues(decimal value, decimal min, decimal max)
        {
            var range = new DecimalRangeAttribute((double)min, (double)max);

            var valid = range.IsValid(value);

            Assert.False(valid);
        }

        [Theory]
        [InlineData(2, 1, 3)] // value is int
        [InlineData(2.0, 1, 3)] // value is double
        [InlineData("2.0", 1, 3)] // value is string
        [InlineData(2.0f, 1, 3)] // value is float
        public void NonDecimalIsInvalid(object value, double min, double max)
        {
            var range = new DecimalRangeAttribute(min, max);

            var valid = range.IsValid(value);

            Assert.False(valid);
        }

        // Should be valid so that RequiredAttribute can do it's job.
        [Fact]
        public void NullIsValid()
        {
            var range = new DecimalRangeAttribute(0, 1);
            decimal? check = null;

            var valid = range.IsValid(check);

            Assert.True(valid);
        }

    }
}
