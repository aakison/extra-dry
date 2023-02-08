namespace ExtraDry.Core.Tests;

public class ArgumentMismatchExceptionTests {

    [Fact]
    public void ArgumentMismatchRoundtrip1()
    {
        var ex = new ArgumentMismatchException("message", "param");

        Assert.StartsWith("message", ex.Message);
        Assert.Contains("param", ex.Message);
        Assert.Equal("param", ex.ParamName);
        Assert.Contains("param", ex.UserMessage);
        Assert.Contains("entity occurs in both the URI and in the body", ex.UserMessage);
        Assert.Null(ex.InnerException);
    }

    [Fact]
    public void ArgumentMismatchRoundtrip2()
    {
        var ex = new ArgumentMismatchException("message", "param", "userMessage");

        Assert.StartsWith("message", ex.Message);
        Assert.Contains("param", ex.Message);
        Assert.Equal("param", ex.ParamName);
        Assert.Equal("userMessage", ex.UserMessage);
        Assert.Null(ex.InnerException);
    }

    [Fact]
    public void ArgumentMismatchRoundtrip3()
    {
        var inner = new Exception("inner");
        var ex = new ArgumentMismatchException("message", inner);

        Assert.StartsWith("message", ex.Message);
        Assert.Null(ex.ParamName);
        Assert.Equal(inner, ex.InnerException);
    }

}

