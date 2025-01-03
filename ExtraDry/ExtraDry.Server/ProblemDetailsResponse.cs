using ExtraDry.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;
using System.Net;

namespace ExtraDry.Server;

public static class ProblemDetailsResponse
{
    internal static void RewriteResponse(ExceptionContext context, HttpStatusCode httpStatusCode, string? title = null, string? details = null)
    {
        RewriteResponse(context.HttpContext, context.Exception.GetType().Name, (int)httpStatusCode, title ?? context.Exception.Message, details);
    }

    internal static void RewriteResponse(HttpContext httpContext, HttpStatusCode httpStatusCode, string? title = null, string? details = null)
    {
        string statusCode = httpStatusCode.ToString();
        RewriteResponse(httpContext, statusCode.ToLower(CultureInfo.InvariantCulture), httpContext.Response.StatusCode, title ?? statusCode, details);
    }

    private static void RewriteResponse(HttpContext httpContext, string problem, int code, string title, string? details = null)
    {
        var problemDetails = CreateProblemDetails(httpContext, problem, code, title, details);
        var body = JsonSerializer.Serialize(problemDetails, SerializerOptions);
        var response = httpContext.Response;
        response.Clear();
        response.StatusCode = code;
        response.ContentType = "application/problem+json";
        response.ContentLength = body.Length;
        response.WriteAsync(body);
    }

    private static ProblemDetails CreateProblemDetails(HttpContext httpContext, string problem, int code, string title, string? details = null)
    {
        var kebab = DataConverter.CamelCaseToKebabCase(problem);
        var url = $"https://{httpContext.Request.Host}/problems/{kebab}";

        var problemDetails = new ProblemDetails {
            Type = url,
            Status = code,
            Title = title,
            Detail = details,
            Instance = httpContext.Request.Path
        };

        return problemDetails;
    }

    private static JsonSerializerOptions SerializerOptions { get; } =
        new(JsonSerializerDefaults.Web) { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault };
}
