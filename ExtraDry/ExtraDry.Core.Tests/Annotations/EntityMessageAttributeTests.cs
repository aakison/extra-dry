using ExtraDry.Core.Models;

namespace ExtraDry.Core.Tests;

public class EntityMessageTests
{

    [Theory]
    [InlineData("EntityName", "abc")]
    [InlineData("EntityName", "")]
    public void RoundtripFormatProperty(string propertyName, string propertyValue)
    {
        var entityMessage = new EntityMessage(propertyValue);
        var property = entityMessage.GetType().GetProperty(propertyName);

        var result = property?.GetValue(entityMessage);

        Assert.Equal(propertyValue, result);
    }

    [Theory]
    [InlineData("EntityName", "abc")]
    [InlineData("EntityName", "")]
    public void RoundtripEntityMessageProperty(string propertyName, string propertyValue)
    {
        var entityMessage = new EntityMessage("different");
        var property = entityMessage.GetType().GetProperty(propertyName);

        property?.SetValue(entityMessage, propertyValue);
        var result = property?.GetValue(entityMessage);

        Assert.Equal(propertyValue, result);
    }

}
