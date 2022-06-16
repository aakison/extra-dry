namespace ExtraDry.Server.Tests.Models;

public class ErrorResponseTests {

    [Fact]
    public void Defaults()
    {
        var response = ValidResponse;

        Assert.Equal(400, response.StatusCode);
        Assert.Empty(response.Description);
        Assert.Empty(response.Display);
        Assert.Empty(response.DisplayCode);
    }

    [Theory]
    [InlineData(nameof(ErrorResponse.StatusCode), 503)]
    [InlineData(nameof(ErrorResponse.Description), "Any")]
    [InlineData(nameof(ErrorResponse.Display), "Any")]
    [InlineData(nameof(ErrorResponse.DisplayCode), "Any")]
    public void RoundtripProperties(string propertyName, object propertyValue)
    {
        var target = ValidResponse;
        var property = target.GetType().GetProperty(propertyName)!;

        property.SetValue(target, propertyValue);
        var result = property.GetValue(target);

        Assert.Equal(propertyValue, result);
    }

    private ErrorResponse ValidResponse => new();

}
