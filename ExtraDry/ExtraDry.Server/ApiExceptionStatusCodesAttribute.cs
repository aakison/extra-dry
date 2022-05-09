using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security;

namespace ExtraDry.Server;

[AttributeUsage(AttributeTargets.Class)]
public class ApiExceptionStatusCodesAttribute : ExceptionFilterAttribute {

    public override Task OnExceptionAsync(ExceptionContext context)
    {
        return base.OnExceptionAsync(context);
    }

    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);
        if(context.Exception is ArgumentOutOfRangeException) {
            var response = context.HttpContext.Response;
            ExceptionResponse.RewriteResponse(response, HttpStatusCode.NotFound, "Not Found", context.Exception.Message);
            context.ExceptionHandled = true;
        }
        else if(context.Exception is ArgumentException || context.Exception is ArgumentNullException) {
            var response = context.HttpContext.Response;
            ExceptionResponse.RewriteResponse(response, HttpStatusCode.BadRequest, "Bad Request", context.Exception.Message);
            context.ExceptionHandled = true;
        }
        else if(context.Exception is NotImplementedException) {
            var response = context.HttpContext.Response;
            ExceptionResponse.RewriteResponse(response, HttpStatusCode.NotImplemented, "Not Implemented", context.Exception.Message);
            context.ExceptionHandled = true;
        }
        else if(context.Exception is SecurityException) {
            var response = context.HttpContext.Response;
            ExceptionResponse.RewriteResponse(response, HttpStatusCode.Forbidden, "Forbidden", context.Exception.Message);
            context.ExceptionHandled = true;
        }
        else if(context.Exception is DryException dryException) {
            var response = context.HttpContext.Response;
            ExceptionResponse.RewriteResponse(response, HttpStatusCode.BadRequest, dryException.UserMessage ?? "Unspecified", dryException.Message);
            context.ExceptionHandled = true;
        }
    }

}
