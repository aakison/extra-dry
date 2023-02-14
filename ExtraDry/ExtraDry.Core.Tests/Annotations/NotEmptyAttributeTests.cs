namespace ExtraDry.Core.Tests;

public class NotEmptyAttributeTests {

    [Fact]
    public void ActualGuidIsNotEmpty()
    {
        var attribute = new NotEmptyAttribute();

        var result = attribute.IsValid(Guid.NewGuid());

        Assert.True(result);
    }

    [Fact]
    public void EmptyGuidIsNotNotEmpty()
    {
        var attribute = new NotEmptyAttribute();

        var result = attribute.IsValid(Guid.Empty);

        Assert.False(result);
    }

    [Fact]
    public void ActualDecimalIsNotEmpty()
    {
        var attribute = new NotEmptyAttribute();

        var result = attribute.IsValid(1.0m);

        Assert.True(result);
    }

    [Fact]
    public void EmptyDecimalIsNotNotEmpty()
    {
        var attribute = new NotEmptyAttribute();

        var result = attribute.IsValid(0.0m);

        Assert.False(result);
    }

    [Fact]
    public void NullIsNotNotEmpty()
    {
        var attribute = new NotEmptyAttribute();

        var result = attribute.IsValid(null);

        Assert.True(result);
    }

    [Fact]
    public void ArbitraryObjectIsNotNotEmpty()
    {
        var attribute = new NotEmptyAttribute();

        var result = attribute.IsValid(new object());

        Assert.True(result);
    }

}
