using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ExtraDry.Server;

internal static class ExceptionResponse {

    public static void RewriteResponse(ExceptionContext context, HttpStatusCode code, string? details = null)
    {
        var kebab = DataConverter.CamelCaseToKebabCase(context.Exception.GetType().Name);
        var url = $"https://{context.HttpContext.Request.Host}/problems/{kebab}";
        var uri = context.HttpContext.Request.Path;
        var er = new ProblemDetails {
            Type = url,
            Status = (int)code,
            Title = context.Exception.Message,
            Detail = details,
            Instance = uri,
        };
        var body = JsonSerializer.Serialize(er);
        var response = context.HttpContext.Response;
        response.Clear();
        response.StatusCode = (int)code;
        response.ContentType = "application/problem+json";
        response.ContentLength = body.Length;
        response.WriteAsync(body);
    }

}
