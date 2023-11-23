using ExtraDry.Core.Models;
using System.Net;

namespace ExtraDry.Core.Tests;

public class DryExceptionTests
{

    [Fact]
    public void ArgumentMismatchRoundtrip1()
    {
        var ex = new DryException();

        Assert.StartsWith("Error in the application", ex.Message);
        Assert.Null(ex.ProblemDetails.Detail);
        Assert.Null(ex.ProblemDetails.Instance);
        Assert.Null(ex.ProblemDetails.Title);
        Assert.Null(ex.ProblemDetails.Status);
    }

    [Fact]
    public void ArgumentMismatchRoundtrip2()
    {
        var problem = new ProblemDetails { Detail = "detail", Instance = "instance", Status = 400, Title = "title", Type = "type" };

        var ex = new DryException(problem);

        Assert.Equal("title", ex.Message);
        Assert.Equal("detail", ex.ProblemDetails.Detail);
        Assert.Equal("instance", ex.ProblemDetails.Instance);
        Assert.Equal("title", ex.ProblemDetails.Title);
        Assert.Equal(400, ex.ProblemDetails.Status);
    }

    [Fact]
    public void ArgumentMismatchRoundtrip3()
    {
        var ex = new DryException(HttpStatusCode.BadRequest, "message", "detail");

        Assert.Equal("message", ex.Message);
        Assert.Equal("detail", ex.ProblemDetails.Detail);
        Assert.Null(ex.ProblemDetails.Instance);
        Assert.Equal("message", ex.ProblemDetails.Title);
        Assert.Equal(400, ex.ProblemDetails.Status);
    }

    [Fact]
    public void ArgumentMismatchRoundtrip4()
    {
        var ex = new DryException("message");

        Assert.Equal("message", ex.Message);
        Assert.Null(ex.ProblemDetails.Detail);
        Assert.Null(ex.ProblemDetails.Instance);
        Assert.Equal("message", ex.ProblemDetails.Title);
        Assert.Null(ex.ProblemDetails.Status);
    }

    [Fact]
    public void ArgumentMismatchRoundtrip5()
    {
        var inner = new ArgumentException("inner");

        var ex = new DryException("message", inner);

        Assert.Equal("message", ex.Message);
        Assert.Null(ex.ProblemDetails.Detail);
        Assert.Null(ex.ProblemDetails.Instance);
        Assert.Equal("message", ex.ProblemDetails.Title);
        Assert.Null(ex.ProblemDetails.Status);
        Assert.Equal(inner, ex.InnerException);
    }

    [Fact]
    public void ArgumentMismatchRoundtrip6()
    {
        var ex = new DryException("message", "detail");

        Assert.Equal("message", ex.Message);
        Assert.Equal("detail", ex.ProblemDetails.Detail);
        Assert.Null(ex.ProblemDetails.Instance);
        Assert.Equal("message", ex.ProblemDetails.Title);
        Assert.Null(ex.ProblemDetails.Status);
    }

}

