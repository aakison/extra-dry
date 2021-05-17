using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.ExtraDry.Tests.Internals {
    public class ContinuationTokenTests {

        [Fact]
        public void DefaultValues()
        {
            var token = new ContinuationToken();

            Assert.True(token.Ascending);
            Assert.Equal(string.Empty, token.Filter);
            Assert.Equal(string.Empty, token.Sort);
            Assert.Equal(string.Empty, token.Stabalizer);
            Assert.Equal(0, token.Skip);
            Assert.Equal(0, token.Take);
        }

        [Fact]
        public void InitializerWithValues()
        {
            var token = new ContinuationToken("filter", "sort", false, "stabalizer", 10, 20);

            Assert.False(token.Ascending);
            Assert.Equal("filter", token.Filter);
            Assert.Equal("sort", token.Sort);
            Assert.Equal("stabalizer", token.Stabalizer);
            Assert.Equal(10, token.Skip);
            Assert.Equal(20, token.Take);
        }

        [Fact]
        public void RoundtripToken()
        {
            var token = new ContinuationToken("filter", "sort", false, "stabalizer", 10, 20);

            var serial = token.ToString();
            var result = ContinuationToken.FromString(serial);

            Assert.Equal(token.Ascending, result.Ascending);
            Assert.Equal(token.Filter, result.Filter);
            Assert.Equal(token.Sort, result.Sort);
            Assert.Equal(token.Stabalizer, result.Stabalizer);
            Assert.Equal(token.Skip, result.Skip);
            Assert.Equal(token.Take, result.Take);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreatesDefaultToken(string serial)
        {
            var token = ContinuationToken.FromString(serial);

            Assert.Null(token);
        }

        [Theory]
        [InlineData("NotAValidToken")] // not even Base64
        [InlineData("VGhpcyBpcyBub3QgYSB0b2tlbg==")] // Valid Base64, but not valid token.
        public void InvalidToken(string serial)
        {
            Assert.Throws<DryException>(() => ContinuationToken.FromString(serial));
        }

        [Fact]
        public void SingleTokenCaching()
        {
            var serial = "AAAAAMgAAABkAAAA";
            var token1 = ContinuationToken.FromString(serial);

            var token2 = ContinuationToken.FromString(serial);

            Assert.Same(token1, token2);
        }

        [Theory]
        [InlineData(0, PageQuery.DefaultTake)]
        [InlineData(10, 10)]
        [InlineData(-1, PageQuery.DefaultTake)]
        [InlineData(1000, 1000)]
        public void TakeSizeForNoToken(int take, int expected)
        {
            var actual = ContinuationToken.ActualTake(null, take);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 13)]
        [InlineData(10, 10)]
        [InlineData(-1, 13)]
        [InlineData(1000, 1000)]
        public void TakeSizeForToken(int take, int expected)
        {
            var token = new ContinuationToken("", "", false, "", 12, 13);
            var actual = ContinuationToken.ActualTake(token, take);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(10, 10)]
        [InlineData(-1, 0)]
        [InlineData(1000, 1000)]
        public void SkipSizeForNoToken(int skip, int expected)
        {
            var actual = ContinuationToken.ActualSkip(null, skip);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 12)]
        [InlineData(10, 10)]
        [InlineData(-1, 12)]
        [InlineData(1000, 1000)]
        public void SkipSizeForToken(int skip, int expected)
        {
            var token = new ContinuationToken("", "", false, "", 12, 13);
            var actual = ContinuationToken.ActualSkip(token, skip);

            Assert.Equal(expected, actual);
        }

    }
}
