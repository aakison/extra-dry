namespace ExtraDry.Core.Tests;

public class NavigationAttributeTests {

    [Theory]
    [InlineData("Group", "abc")]
    [InlineData("Group", "")]
    [InlineData("Icon", "abc")]
    [InlineData("Icon", "")]
    [InlineData("Icon", null)]
    [InlineData("Title", "abc")]
    [InlineData("Title", "")]
    [InlineData("Subtitle", "abc")]
    [InlineData("Subtitle", "")]
    [InlineData("ActiveMatch", "abc")]
    [InlineData("ActiveMatch", "")]
    [InlineData("ActiveMatch", null)]
    [InlineData("Order", 10)]
    public void RoundtripNavigationProperty(string propertyName, object propertyValue)
    {
        var attribute = new NavigationAttribute();
        var property = attribute.GetType().GetProperty(propertyName);

        property?.SetValue(attribute, propertyValue);
        var result = property?.GetValue(attribute);

        Assert.Equal(propertyValue, result);
    }

    [Theory]
    [InlineData("Title", "abc")]
    [InlineData("Title", "")]
    public void NavigationConstructor(string propertyName, string propertyValue)
    {
        var attribute = new NavigationAttribute(propertyValue);
        var property = attribute.GetType().GetProperty(propertyName);

        var result = property?.GetValue(attribute);

        Assert.Equal(propertyValue, result);
    }

}
