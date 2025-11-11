using Xunit;
using ExtraDry.Blazor.Components.Formatting;
using System.Globalization;

namespace ExtraDry.Blazor.Tests.Components.Formatting;

public class NullableDateTimeFormatterTests
{
    [Fact]
    public void FormatNull()
    {
        var formatter = new NullableDateTimeFormatter();
        var result = formatter.Format(null);
        Assert.Equal("", result);
    }

    [Fact]
    public void FormatMinValue()
    {
        var formatter = new NullableDateTimeFormatter();
        var result = formatter.Format(DateTime.MinValue);
        Assert.Equal("", result);
    }

    [Fact]
    public void FormatValidDateTime()
    {
        var formatter = new NullableDateTimeFormatter();
        var result = formatter.Format(new DateTime(2023, 10, 15));
        Assert.Equal("2023-10-15", result);
    }

    [Fact]
    public void TryParseNull()
    {
        var formatter = new NullableDateTimeFormatter();
        var success = formatter.TryParse(null, out var result);
        Assert.True(success);
        Assert.Null(result);
    }

    [Fact]
    public void TryParseEmpty()
    {
        var formatter = new NullableDateTimeFormatter();
        var success = formatter.TryParse("", out var result);
        Assert.True(success);
        Assert.Null(result);
    }

    [Fact]
    public void TryParseWhitespace()
    {
        var formatter = new NullableDateTimeFormatter();
        var success = formatter.TryParse("   ", out var result);
        Assert.True(success);
        Assert.Null(result);
    }

    [Fact]
    public void TryParseValid()
    {
        var formatter = new NullableDateTimeFormatter();
        var success = formatter.TryParse("2023-10-15", out var result);
        Assert.True(success);
        Assert.Equal(new DateTime(2023, 10, 15), result);
    }

    [Fact]
    public void TryParseInvalid()
    {
        var formatter = new NullableDateTimeFormatter();
        var success = formatter.TryParse("abc", out var result);
        Assert.False(success);
        Assert.Null(result);
    }
}
