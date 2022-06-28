using Microsoft.AspNetCore.Http;
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
        if(context.Exception is ArgumentMismatchException ex) {
            ExceptionResponse.RewriteResponse(context, HttpStatusCode.BadRequest, ex.UserMessage);
            context.ExceptionHandled = true;
        }
        else if(context.Exception is ArgumentOutOfRangeException) {
            ExceptionResponse.RewriteResponse(context, HttpStatusCode.NotFound);
            context.ExceptionHandled = true;
        }
        else if(context.Exception is ArgumentException || context.Exception is ArgumentNullException) {
            ExceptionResponse.RewriteResponse(context, HttpStatusCode.BadRequest);
            context.ExceptionHandled = true;
        }
        else if(context.Exception is NotImplementedException) {
            ExceptionResponse.RewriteResponse(context, HttpStatusCode.NotImplemented);
            context.ExceptionHandled = true;
        }
        else if(context.Exception is SecurityException) {
            ExceptionResponse.RewriteResponse(context, HttpStatusCode.Forbidden);
            context.ExceptionHandled = true;
        }
        else if(context.Exception is DryException dryException) {
            ExceptionResponse.RewriteResponse(context, HttpStatusCode.BadRequest, dryException.UserMessage);
            context.ExceptionHandled = true;
        }
    }



}
