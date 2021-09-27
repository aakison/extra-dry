#nullable enable

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.Server.Security {

    /// <summary>
    /// During construction of the SwaggerGen, use the Authorize attribute to determine if Basic Auth required.
    /// </summary>
    public class BasicAuthOperationFilter : IOperationFilter {

        /// <summary>
        /// Scan through each operation, using Authorize to add security requirement to OpenApi.
        /// </summary>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attributes = context.MethodInfo.GetCustomAttributes(true) ?? Array.Empty<object>();

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

        }

    }

}
