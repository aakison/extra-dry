using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ExtraDry.Server;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ExceptionStatusCodeAttribute(
    Type exceptionType,
    HttpStatusCode code)
    : ExceptionFilterAttribute
{
    public override Task OnExceptionAsync(ExceptionContext context)
    {
        return base.OnExceptionAsync(context);
    }

    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);
        if(context.Exception.GetType().Name == exceptionType.Name) {
            ProblemDetailsResponse.RewriteResponse(context, code, context.Exception.Message);
            context.ExceptionHandled = true;
        }
    }
}
