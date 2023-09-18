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
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.BadRequest, ex.UserMessage);
            context.ExceptionHandled = true;
        }
        else if(context.Exception is ArgumentOutOfRangeException || context.Exception is KeyNotFoundException) {
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.NotFound);
            context.ExceptionHandled = true;
        }
        else if(context.Exception is ArgumentException || context.Exception is ArgumentNullException) {
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.BadRequest);
            context.ExceptionHandled = true;
        }
        else if(context.Exception is NotImplementedException) {
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.NotImplemented);
            context.ExceptionHandled = true;
        }
        else if(context.Exception is SecurityException) {
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.Forbidden);
            context.ExceptionHandled = true;
        }
        else if(context.Exception is DryException dryException) {
            // TODO: better handling here...
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.BadRequest, dryException.ProblemDetails.Detail);
            context.ExceptionHandled = true;
        }
        else {
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.InternalServerError);
            context.ExceptionHandled = true;
        }
    }

}
