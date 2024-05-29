using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using System.ComponentModel;
using System.Net;
using System.Security;

namespace ExtraDry.Server;

[AttributeUsage(AttributeTargets.Class)]
public class ApiExceptionStatusCodesAttribute : ExceptionFilterAttribute 
{

    public override Task OnExceptionAsync(ExceptionContext context)
    {
        return base.OnExceptionAsync(context);
    }

    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);
        if(context.Exception is ArgumentMismatchException ex) {
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.BadRequest, ex.UserMessage);
        }
        else if(context.Exception is ArgumentOutOfRangeException || context.Exception is KeyNotFoundException) {
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.NotFound);
        }
        else if(context.Exception is ArgumentException || context.Exception is ArgumentNullException) {
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.BadRequest);
        }
        else if(context.Exception is ValidationException ve) {
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.BadRequest, "One or more validation errors occurred.", ve.Message);
        }
        else if(context.Exception is NotImplementedException) {
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.NotImplemented);
        }
        else if(context.Exception is SecurityException) {
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.Forbidden);
        }
        else if(context.Exception is DryException dryException) {
            // TODO: better handling here...
            int code = dryException.ProblemDetails.Status ?? (int)HttpStatusCode.BadRequest;
            ProblemDetailsResponse.RewriteResponse(context, (HttpStatusCode)code, 
                dryException.ProblemDetails.Title, dryException.ProblemDetails.Detail);
        }
        else if(context.Exception is UnauthorizedAccessException) {
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.Forbidden);
        }
        else {
            ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.InternalServerError, context.Exception.Message, context.Exception.StackTrace);
        }
        context.ExceptionHandled = true;
    }

}
