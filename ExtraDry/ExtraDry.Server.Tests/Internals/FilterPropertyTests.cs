using ExtraDry.Server.Internal;

namespace ExtraDry.Server.Tests.Internals;

public class FilterPropertyTests {

    [Fact]
    public void DefaultValues()
    {
        var property = GetType().GetProperty(nameof(ValidProperty))!;
        var attribute = new FilterAttribute();

        var filter = new FilterProperty(property, attribute);

        Assert.Equal(property, filter.Property);
        Assert.Equal(attribute, filter.Filter);
        Assert.Equal(property.Name, filter.ExternalName);
    }

    [Theory]
    [InlineData("Property", null)]
    [InlineData("Filter", null)]
    public void RoundtripProperties(string propertyName, object? propertyValue)
    {
        var filter = FilterProperty;
        var property = filter.GetType().GetProperty(propertyName);

        property?.SetValue(filter, propertyValue);
        var result = property?.GetValue(filter);

        Assert.Equal(propertyValue, result);
    }

    public string ValidProperty { get; set; } = string.Empty;

    private FilterProperty FilterProperty {
        get {
            var property = GetType().GetProperty(nameof(ValidProperty));
            var filter = new FilterAttribute();
            return new FilterProperty(property!, filter);
        }
    } 

}
