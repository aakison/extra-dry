using Xunit;
using ExtraDry.Blazor.Components.Formatting;

namespace ExtraDry.Blazor.Tests.Components.Formatting;

public class NullableIntFormatterTests
{
    [Fact]
    public void FormatNull()
    {
        var formatter = new NullableIntFormatter();
        var result = formatter.Format(null);
        Assert.Equal("", result);
    }

    [Fact]
    public void FormatZero()
    {
        var formatter = new NullableIntFormatter();
        var result = formatter.Format(0);
        Assert.Equal("0", result);
    }

    [Fact]
    public void FormatPositive()
    {
        var formatter = new NullableIntFormatter();
        var result = formatter.Format(1234);
        Assert.Equal("1,234", result);
    }

    [Fact]
    public void FormatNegative()
    {
        var formatter = new NullableIntFormatter();
        var result = formatter.Format(-1234);
        Assert.Equal("-1,234", result);
    }

    [Fact]
    public void TryParseNull()
    {
        var formatter = new NullableIntFormatter();
        var success = formatter.TryParse(null, out var result);
        Assert.True(success);
        Assert.Null(result);
    }

    [Fact]
    public void TryParseEmpty()
    {
        var formatter = new NullableIntFormatter();
        var success = formatter.TryParse("", out var result);
        Assert.True(success);
        Assert.Null(result);
    }

    [Fact]
    public void TryParseWhitespace()
    {
        var formatter = new NullableIntFormatter();
        var success = formatter.TryParse("   ", out var result);
        Assert.True(success);
        Assert.Null(result);
    }

    [Fact]
    public void TryParseValid()
    {
        var formatter = new NullableIntFormatter();
        var success = formatter.TryParse("1234", out var result);
        Assert.True(success);
        Assert.Equal(1234, result);
    }

    [Fact]
    public void TryParseWithCommas()
    {
        var formatter = new NullableIntFormatter();
        var success = formatter.TryParse("1,234", out var result);
        Assert.True(success);
        Assert.Equal(1234, result);
    }

    [Fact]
    public void TryParseInvalid()
    {
        var formatter = new NullableIntFormatter();
        var success = formatter.TryParse("abc", out var result);
        Assert.False(success);
        Assert.Null(result);
    }
}
