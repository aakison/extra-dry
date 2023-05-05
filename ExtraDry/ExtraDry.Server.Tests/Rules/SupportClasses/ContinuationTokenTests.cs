using ExtraDry.Server.Internal;

namespace ExtraDry.Server.Tests.Rules;

public class ContinuationTokenTests {

    [Fact]
    public void DefaultValues()
    {
        var token = new ContinuationToken();

        Assert.Equal(string.Empty, token.Filter);
        Assert.Equal(string.Empty, token.Sort);
        Assert.Equal(0, token.Skip);
        Assert.Equal(0, token.Take);
    }

    [Fact]
    public void InitializerWithValues()
    {
        var token = new ContinuationToken("filter", "sort", 10, 20);

        Assert.Equal("filter", token.Filter);
        Assert.Equal("sort", token.Sort);
        Assert.Equal(10, token.Skip);
        Assert.Equal(20, token.Take);
    }

    [Fact]
    public void RoundtripToken()
    {
        var token = new ContinuationToken("filter", "sort", 10, 20);

        var serial = token.ToString();
        var result = ContinuationToken.FromString(serial) ?? throw new Exception();

        Assert.Equal(token.Filter, result.Filter);
        Assert.Equal(token.Sort, result.Sort);
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
        var token = new ContinuationToken("", "", 12, 13);
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
        var token = new ContinuationToken("", "", 12, 13);
        var actual = ContinuationToken.ActualSkip(token, skip);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("abc", "abc")]
    public void FilterValueForToken(string input, string expected)
    {
        var token = new ContinuationToken(input, "", 12, 13);
        var actual = token.Filter;

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("abc", "abc")]
    public void SortValueForToken(string input, string expected)
    {
        var token = new ContinuationToken("", input, 12, 13);
        var actual = token.Sort;

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(-1, -1, 20, 10)]
    [InlineData(10, 10, 20, 10)]
    [InlineData(-1, 20, 30, 20)]
    [InlineData(20, -1, 30, 10)]
    [InlineData(20, 20, 40, 20)]
    public void NextToken(int skip, int take, int expectedSkip, int expectedTake)
    {
        var token = new ContinuationToken("filter", "sort", 10, 10);

        var next = token.Next(skip, take);

        Assert.Equal("filter", next.Filter);
        Assert.Equal("sort", next.Sort);
        Assert.Equal(expectedSkip, next.Skip);
        Assert.Equal(expectedTake, next.Take);
    }

}
