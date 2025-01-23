namespace ExtraDry.Server.Tests.Rules;

public class ControlAttributeTests
{
    [Fact]
    public void ValidateControlAttribute()
    {
        var control = ValidControlAttribute;
        var validator = new DataValidator();

        validator.ValidateObject(control);

        Assert.Empty(validator.Errors);
    }

    [Theory]
    [InlineData("Type", ControlType.BestMatch)]
    [InlineData("Icon", "")]
    public void DefaultValues(string propertyName, object defaultValue)
    {
        var control = ValidControlAttribute;
        var property = control.GetType().GetProperty(propertyName);

        var result = property?.GetValue(control);

        Assert.Equal(defaultValue, result);
    }

    [Theory]
    [InlineData("Type", ControlType.RadioButtons)]
    [InlineData("Icon", "X")]
    public void RoundtripProperties(string propertyName, object propertyValue)
    {
        var control = ValidControlAttribute;
        var property = control.GetType().GetProperty(propertyName);

        property?.SetValue(control, propertyValue);
        var result = property?.GetValue(control);

        Assert.Equal(propertyValue, result);
    }

    private static ControlAttribute ValidControlAttribute => new();
}
