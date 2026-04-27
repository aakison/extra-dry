using Microsoft.AspNetCore.Http;
using System.Net;

namespace ExtraDry.Server;

/// <summary>
/// Most problem details are returned from the ApiExceptionStatusCodesAttribute, but sometimes the
/// authorization issue is detected before it can catch it.  Handle these additional cases here.
/// </summary>
public class EarlyAuthorizationFailureMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        switch((HttpStatusCode)context.Response.StatusCode) {
            case HttpStatusCode.Forbidden:
                var policyDetails = AuthorizationPolicyDetailsHelper.GetAuthorizationPolicyDetails(context);
                await ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.Forbidden, details: policyDetails);
                break;

            case HttpStatusCode.Unauthorized:
                await ProblemDetailsResponse.RewriteResponse(context, HttpStatusCode.Unauthorized);
                break;
        }
    }

}
