using Xunit;
using ExtraDry.Blazor.Components.Formatting;

namespace ExtraDry.Blazor.Tests.Components.Formatting;

public class IdentityFormatterTests
{
    [Fact]
    public void FormatNull()
    {
        var formatter = new IdentityFormatter();
        var result = formatter.Format(null);
        Assert.Equal("", result);
    }

    [Fact]
    public void FormatString()
    {
        var formatter = new IdentityFormatter();
        var result = formatter.Format("test");
        Assert.Equal("test", result);
    }

    [Fact]
    public void FormatObject()
    {
        var formatter = new IdentityFormatter();
        var result = formatter.Format(123);
        Assert.Equal("123", result);
    }

    [Fact]
    public void TryParseNull()
    {
        var formatter = new IdentityFormatter();
        var success = formatter.TryParse(null, out var result);
        Assert.True(success);
        Assert.Null(result);
    }

    [Fact]
    public void TryParseEmpty()
    {
        var formatter = new IdentityFormatter();
        var success = formatter.TryParse("", out var result);
        Assert.True(success);
        Assert.Equal("", result);
    }

    [Fact]
    public void TryParseString()
    {
        var formatter = new IdentityFormatter();
        var success = formatter.TryParse("test", out var result);
        Assert.True(success);
        Assert.Equal("test", result);
    }
}
