using System.Text.Json;

namespace ExtraDry.Core.Tests;

public class ExpandoValuesConverterTests
{

    [Fact]
    public void NullExpandoValuesAreNull()
    {
        var json = $@"{{ ""Values"" : null }}";

        var result = JsonSerializer.Deserialize<Target>(json);

        Assert.NotNull(result);
        Assert.Null(result.Values);
    }

    [Fact]
    public void ArrayExpandoValueNotSupported()
    {
        var json = $@"{{ ""Values"" : {{ ""key"" : [1, 2] }} }}";

        Target lambda() => JsonSerializer.Deserialize<Target>(json)!;

        Assert.Throws<DryException>((Func<Target>)lambda);
    }

    [Fact]
    public void ObjectExpandoValueNotSupported()
    {
        var json = $@"{{ ""Values"" : {{ ""key"" : {{ ""inner"" : ""value"" }} }} }}";

        Target lambda() => JsonSerializer.Deserialize<Target>(json)!;

        Assert.Throws<DryException>((Func<Target>)lambda);
    }

    [Fact]
    public void DateTimeExpandoValueConverted()
    {
        var json = $@"{{ ""Values"" : {{ ""key"" : ""2020-02-02T00:00:00"" }} }}";

        var target = JsonSerializer.Deserialize<Target>(json);

        Assert.NotNull(target);
        Assert.IsType<DateTime>(target.Values.First().Value);
    }

    [Theory]
    [InlineData("12.5", 12.5)] // Number => double
    [InlineData("12", 12.0)] // Int Number => double
    [InlineData("true", true)] // Boolean => bool
    [InlineData("false", false)] // Boolean => bool
    [InlineData(@"""Huzzah""", "Huzzah")] // Text => string
    [InlineData("null", null)] // Null => null
    public void CommonTypeExpandoValueRoundtrip(string serialized, object element)
    {
        var json = $@"{{ ""Values"" : {{ ""key"" : {serialized} }} }}";

        var target = JsonSerializer.Deserialize<Target>(json);

        Assert.NotNull(target);
        Assert.Equal(element, target.Values.First().Value);
    }

    public class Target
    {

        public ExpandoValues Values { get; set; } = new();

    }
}
