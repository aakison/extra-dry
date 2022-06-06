using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExtraDry.Core.Tests.Server.Models {
    public class ErrorResponseTests {

        [Fact]
        public void Defaults()
        {
            var response = ValidResponse;

            Assert.Equal(400, response.StatusCode);
            Assert.Empty(response.Description);
            Assert.Empty(response.Display);
            Assert.Empty(response.DisplayCode);
        }

        [Theory]
        [InlineData("StatusCode", 503)]
        [InlineData("Description", "Any")]
        [InlineData("Display", "Any")]
        [InlineData("DisplayCode", "Any")]
        public void RoundtripProperties(string propertyName, object propertyValue)
        {
            var target = ValidResponse;
            var property = target.GetType().GetProperty(propertyName);

            property.SetValue(target, propertyValue);
            var result = property.GetValue(target);

            Assert.Equal(propertyValue, result);
        }

        private ErrorResponse ValidResponse => new();

    }
}
