using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security;

namespace ExtraDry.Server;

[AttributeUsage(AttributeTargets.Class)]
public class ApiExceptionStatusCodesAttribute : ExceptionFilterAttribute
{
    public override async Task OnExceptionAsync(ExceptionContext context)
    {
        if(context.Exception is ArgumentMismatchException ex) {
            await ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.BadRequest, ex.UserMessage);
        }
        else if(context.Exception is ArgumentOutOfRangeException or KeyNotFoundException) {
            await ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.NotFound);
        }
        else if(context.Exception is ArgumentException or ArgumentNullException or InvalidOperationException) {
            await ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.BadRequest);
        }
        else if(context.Exception is ValidationException ve) {
            await ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.BadRequest, "One or more validation errors occurred.", ve.Message);
        }
        else if(context.Exception is NotImplementedException) {
            await ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.NotImplemented);
        }
        else if(context.Exception is SecurityException) {
            var policyDetails = AuthorizationPolicyDetailsHelper.GetAuthorizationPolicyDetails(context.HttpContext);
            await ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.Forbidden, details: policyDetails);
        }
        else if(context.Exception is DryException dryException) {
            int code = dryException.ProblemDetails.Status ?? (int)HttpStatusCode.BadRequest;
            await ProblemDetailsResponse.RewriteResponse(context, (HttpStatusCode)code,
                dryException.ProblemDetails.Title, dryException.ProblemDetails.Detail);
        }
        else if(context.Exception is UnauthorizedAccessException) {
            var policyDetails = AuthorizationPolicyDetailsHelper.GetAuthorizationPolicyDetails(context.HttpContext);
            await ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.Forbidden, details: policyDetails);
        }
        else {
            await ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.InternalServerError, context.Exception.Message, context.Exception.StackTrace);
        }
        context.ExceptionHandled = true;
    }
}

