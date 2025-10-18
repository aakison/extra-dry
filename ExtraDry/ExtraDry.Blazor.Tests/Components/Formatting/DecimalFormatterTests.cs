using Xunit;
using ExtraDry.Blazor.Components.Formatting;

namespace ExtraDry.Blazor.Tests.Components.Formatting;

public class DecimalFormatterTests
{
    [Fact]
    public void FormatNull()
    {
        var formatter = new DecimalFormatter();
        var result = formatter.Format(null);
        Assert.Equal("0.00", result);
    }

    [Fact]
    public void FormatZero()
    {
        var formatter = new DecimalFormatter();
        var result = formatter.Format(0m);
        Assert.Equal("0.00", result);
    }

    [Fact]
    public void FormatPositive()
    {
        var formatter = new DecimalFormatter();
        var result = formatter.Format(1234.56m);
        Assert.Equal("1,234.56", result);
    }

    [Fact]
    public void FormatNegative()
    {
        var formatter = new DecimalFormatter();
        var result = formatter.Format(-1234.56m);
        Assert.Equal("-1,234.56", result);
    }

    [Fact]
    public void TryParseNull()
    {
        var formatter = new DecimalFormatter();
        var success = formatter.TryParse(null, out var result);
        Assert.True(success);
        Assert.Equal(0m, result);
    }

    [Fact]
    public void TryParseEmpty()
    {
        var formatter = new DecimalFormatter();
        var success = formatter.TryParse("", out var result);
        Assert.True(success);
        Assert.Equal(0m, result);
    }

    [Fact]
    public void TryParseWhitespace()
    {
        var formatter = new DecimalFormatter();
        var success = formatter.TryParse("   ", out var result);
        Assert.True(success);
        Assert.Equal(0m, result);
    }

    [Fact]
    public void TryParseValid()
    {
        var formatter = new DecimalFormatter();
        var success = formatter.TryParse("1234.56", out var result);
        Assert.True(success);
        Assert.Equal(1234.56m, result);
    }

    [Fact]
    public void TryParseWithCommas()
    {
        var formatter = new DecimalFormatter();
        var success = formatter.TryParse("1,234.56", out var result);
        Assert.True(success);
        Assert.Equal(1234.56m, result);
    }

    [Fact]
    public void TryParseInvalid()
    {
        var formatter = new DecimalFormatter();
        var success = formatter.TryParse("abc", out var result);
        Assert.False(success);
        Assert.Equal(0m, result);
    }
}
