using Microsoft.AspNetCore.Authorization;

namespace Sample.Shared.Security;

/// <summary>
/// Policies are shared between the back-end API server and the front-end Blazor SPA. This sample
/// shows how to use a single method to register policies on both front and back end.
/// </summary>
public static class SamplePolicies
{
    /// <summary>
    /// Method to be called in startup to register the policies with front-end and back-end. In
    /// Blazor: `builder.Services.AddAuthorizationCore(options =&gt;
    /// SamplePolicies.AddAuthorizationOptions(options));` In MVC:
    /// `services.AddAuthorization(options =&gt; SamplePolicies.AddAuthorizationOptions(options));`
    /// </summary>
    public static void AddAuthorizationOptions(AuthorizationOptions options)
    {
        options.AddPolicy(SamplePolicy, policy => policy.Requirements.Add(new SampleAccessRequirement()));
    }

    /// <summary>
    /// A sample policy constant to be used when defining authorization rules on endpoints. E.g.
    /// use `[Authorize(SamplePolicies.SamplePolicy)]` instead of `[Authorize("SamplePolicy")]`.
    /// </summary>
    public const string SamplePolicy = "SamplePolicy";
}
