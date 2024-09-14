namespace ExtraDry.Server.Tests.Rules;

public class DryExceptionTests {

    [Fact]
    public void DefaultValuesDefaultConstructor()
    {
        var exception = new DryException();

        Assert.Null(exception.ProblemDetails.Detail);
        Assert.Contains("Error in the application", exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void DefaultValuesSingleConstructor()
    {
        var exception = new DryException("message");

        Assert.Null(exception.ProblemDetails.Detail);
        Assert.Contains("message", exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void DefaultValuesInnerConstructor()
    {
        var inner = new ArgumentException();
        var exception = new DryException("message", inner);

        Assert.Null(exception.ProblemDetails.Detail);
        Assert.Contains("message", exception.Message);
        Assert.Equal(inner, exception.InnerException);
    }

    [Fact]
    public void DefaultValuesUserMessageConstructor()
    {
        var exception = new DryException("message", "user");

        Assert.Equal("user", exception.ProblemDetails.Detail);
        Assert.Contains("message", exception.Message);
        Assert.Null(exception.InnerException);
    }

    [Fact]
    public void UserMessageChangable()
    {
        var exception = new DryException("message", "user");
        exception.ProblemDetails.Detail = "new-message";

        Assert.Equal("new-message", exception.ProblemDetails.Detail);
    }

}
