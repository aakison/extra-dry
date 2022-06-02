#nullable enable

using ExtraDry.Core;
using Xunit;

namespace ExtraDry.Core.Tests.Rules.SupportClasses {

    public class ControlAttributeTests {

        [Fact]
        public void ValidateControlAttribute()
        {
            var control = ValidControlAttribute;
            var validator = new DataValidator();

            validator.ValidateObject(control);

            Assert.Empty(validator.Errors);
        }

        [Theory]
        [InlineData("Type", ControlType.BestMatch)]
        [InlineData("CaptionTemplate", "{0}")]
        [InlineData("IconTemplate", "")]
        public void DefaultValues(string propertyName, object defaultValue)
        { 
            var control = ValidControlAttribute;
            var property = control.GetType().GetProperty(propertyName);

            var result = property?.GetValue(control);

            Assert.Equal(defaultValue, result);
        }

        [Theory]
        [InlineData("Type", ControlType.RadioButtons)]
        [InlineData("CaptionTemplate", "X")]
        [InlineData("IconTemplate", "X")]
        public void RoundtripProperties(string propertyName, object propertyValue)
        {
            var control = ValidControlAttribute;
            var property = control.GetType().GetProperty(propertyName);

            property?.SetValue(control, propertyValue);
            var result = property?.GetValue(control);

            Assert.Equal(propertyValue, result);
        }

        private static ControlAttribute ValidControlAttribute => new();

    }
}
