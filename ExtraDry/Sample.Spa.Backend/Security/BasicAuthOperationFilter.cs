using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Sample.Spa.Backend.Security;

/// <summary>
/// During construction of the OpenAPI document, use the Authorize attribute to determine if Basic Auth
/// required.
/// </summary>
public class BasicAuthOperationFilter : IOpenApiOperationTransformer
{
    /// <summary>
    /// Scan through each operation, using Authorize to add security requirement to OpenApi.
    /// </summary>
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var attributes = context.Description.ActionDescriptor.EndpointMetadata;

        var authAttributes = attributes.OfType<AuthorizeAttribute>();
        if(authAttributes.Any()) {
            operation.Security.Add(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "http",
                        },
                        Scheme = "basic",
                        Name = "basic",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        }

        return Task.CompletedTask;
    }
}
