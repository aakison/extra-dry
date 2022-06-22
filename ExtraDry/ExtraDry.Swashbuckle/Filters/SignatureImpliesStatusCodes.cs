using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ExtraDry.Swashbuckle;

/// <summary>
/// During construction of the SwaggerGen, use the Attributes to intuit the likely HTTP response error codes.
/// </summary>
public class SignatureImpliesStatusCodes : IOperationFilter {

    /// <summary>
    /// Scan through each operation, using attribute signatures to guess the typical client errors that will be surfaced.
    /// </summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var schema = context.SchemaGenerator.GenerateSchema(typeof(ErrorResponse), context.SchemaRepository);

        var attributes = context.MethodInfo.GetCustomAttributes(true) ?? Array.Empty<object>();

        var consumesAttributes = attributes.OfType<ConsumesAttribute>();
        if(operation.Parameters.Any() || consumesAttributes.Any()) {
            if(!operation.Responses.ContainsKey("400")) {
                operation.Responses.Add("400", new OpenApiResponse {
                    Description = "Bad Request",
                    Content = {
                        ["application/json"] = new OpenApiMediaType {
                            Schema = schema,
                            Example = OpenApiAnyFactory.CreateFromJson(json400),
                        }
                    }
                });
            }
        }

        var authAttributes = attributes.OfType<AuthorizeAttribute>();
        if(authAttributes.Any()) {
            if(!operation.Responses.ContainsKey("401")) {
                operation.Responses.Add("401", new OpenApiResponse {
                    Description = "Unauthorized",
                    Content = {
                        ["application/json"] = new OpenApiMediaType {
                            Schema = schema,
                            Example = OpenApiAnyFactory.CreateFromJson(json401),
                        }
                    }
                });
            }
            if(!operation.Responses.ContainsKey("403")) {
                operation.Responses.Add("403", new OpenApiResponse {
                    Description = "Forbidden",
                    Content = {
                        ["application/json"] = new OpenApiMediaType {
                            Schema = schema,
                            Example = OpenApiAnyFactory.CreateFromJson(json403),
                        }
                    }
                });
            }
        }

        var methodAttributes = attributes.OfType<HttpMethodAttribute>();
        if(methodAttributes.Any(e => e?.Template?.Contains('{') ?? false)) {
            if(!operation.Responses.ContainsKey("404")) {
                operation.Responses.Add("404", new OpenApiResponse {
                    Description = "Not Found",
                    Content = {
                        ["application/json"] = new OpenApiMediaType {
                            Schema = schema,
                            Example = OpenApiAnyFactory.CreateFromJson(json404),
                        }
                    }
                });
            }
        }

    }

    private const string json400 = @"{
            ""statusCode"": 400,
            ""description"": ""The indicated input could not be parsed as a UUID"",
            ""display"": ""Invalid request, check inputs."",
            ""displayCode"": ""0x0F00B75A""
        }";

    private const string json401 = @"{
            ""statusCode"": 401,
            ""description"": ""The detected authentication mechanism, BasicAuthentication, is not supported.  Please provide a JWT Bearer token."",
            ""display"": ""Unable to login, please check credentials."",
            ""displayCode"": ""0x0F0056CA""
        }";

    private const string json403 = @"{
            ""statusCode"": 403,
            ""description"": ""Valid credentials were supplied by one or more policies were not satisfied."",
            ""display"": ""Access denied, please check credentials."",
            ""displayCode"": ""0x0F0082B1""
        }";

    private const string json404 = @"{
            ""statusCode"": 404,
            ""description"": ""The requested resource was not found, no entity with indicatd UUID exists."",
            ""display"": ""Not found, please refresh."",
            ""displayCode"": ""0x0F0092C1""
        }";
}
