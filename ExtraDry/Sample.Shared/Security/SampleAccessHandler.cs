#nullable enable

using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Sample.Shared.Security {

    /// <summary>
    /// Sample access handler to demonstrate how policies would be authorized.
    /// In this sample, there is no rigorous security and this handler lets the SampleAccessRequirement through
    /// as if it were an anonymous request.
    /// </summary>
    public class SampleAccessHandler : AuthorizationHandler<SampleAccessRequirement> {

        /// <summary>
        /// DI Constructor, use to inject dependencies that are required to assess security requirement.
        /// </summary>
        public SampleAccessHandler()
        {
        }

        /// <summary>
        /// Sample requirement checker, throw `SecurityException` to fail the requirement.
        /// </summary>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SampleAccessRequirement requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

    }
}
