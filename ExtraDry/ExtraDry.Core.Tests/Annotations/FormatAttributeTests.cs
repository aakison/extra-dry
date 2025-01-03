namespace ExtraDry.Core.Tests;

public class FormatAttributeTests
{
    [Theory]
    [InlineData("Icon", "abc")]
    [InlineData("Icon", "")]
    public void RoundtripFormatProperty(string propertyName, string propertyValue)
    {
        var attribute = new FormatAttribute();
        var property = attribute.GetType().GetProperty(propertyName);

        property?.SetValue(attribute, propertyValue);
        var result = property?.GetValue(attribute);

        Assert.Equal(propertyValue, result);
    }
}
