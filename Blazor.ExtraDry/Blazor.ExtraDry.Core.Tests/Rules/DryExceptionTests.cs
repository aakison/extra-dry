#nullable enable

using System;
using Xunit;

namespace Blazor.ExtraDry.Core.Tests.Rules {
    public class DryExceptionTests {

        [Fact]
        public void DefaultValuesDefaultConstructor()
        {
            var exception = new DryException();

            Assert.Null(exception.UserMessage);
            Assert.Contains(nameof(DryException), exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void DefaultValuesSingleConstructor()
        {
            var exception = new DryException("message");

            Assert.Null(exception.UserMessage);
            Assert.Contains("message", exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void DefaultValuesInnerConstructor()
        {
            var inner = new Exception();
            var exception = new DryException("message", inner);

            Assert.Null(exception.UserMessage);
            Assert.Contains("message", exception.Message);
            Assert.Equal(inner, exception.InnerException);
        }

        [Fact]
        public void DefaultValuesUserMessageConstructor()
        {
            var exception = new DryException("message", "user");

            Assert.Equal("user", exception.UserMessage);
            Assert.Contains("message", exception.Message);
            Assert.Null(exception.InnerException);
        }

        [Fact]
        public void UserMessageChangable()
        {
            var exception = new DryException("message", "user");

            exception.UserMessage = "new-message";

            Assert.Equal("new-message", exception.UserMessage);
        }

    }
}
