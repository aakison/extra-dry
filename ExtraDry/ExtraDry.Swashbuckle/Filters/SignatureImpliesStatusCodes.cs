using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net;
using static ExtraDry.Swashbuckle.ExtraDryGenOptions;

namespace ExtraDry.Swashbuckle;

/// <summary>
/// During construction of the SwaggerGen, use the Attributes to intuit the likely HTTP response error codes.
/// </summary>
public class SignatureImpliesStatusCodes : IOperationFilter
{
    /// <summary>
    /// Scan through each operation, using attribute signatures to guess the typical client errors that will be surfaced.
    /// </summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var schema = context.SchemaGenerator.GenerateSchema(typeof(Core.Models.ProblemDetails), context.SchemaRepository);

        var attributes = context.MethodInfo.GetCustomAttributes(true) ?? [];

        var consumesAttributes = attributes.OfType<ConsumesAttribute>();
        if(operation.Parameters.Any() || consumesAttributes.Any()) {
            if(!operation.Responses.ContainsKey("400")) {
                operation.Responses.Add("400", new OpenApiResponse {
                    Description = "Bad Request",
                    Content = {
                        ["application/problem+json"] = new OpenApiMediaType {
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
                        ["application/problem+json"] = new OpenApiMediaType {
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
                        ["application/problem+json"] = new OpenApiMediaType {
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
                        ["application/problem+json"] = new OpenApiMediaType {
                            Schema = schema,
                            Example = OpenApiAnyFactory.CreateFromJson(json404),
                        }
                    }
                });
            }
        }

        UpdateResponse<HttpPostAttribute>(operation, attributes, HttpStatusCode.Created);
        UpdateResponse<HttpDeleteAttribute>(operation, attributes, HttpStatusCode.NoContent, "Success");
    }

    private static void UpdateResponse<T>(OpenApiOperation operation, object[] attributes, HttpStatusCode httpStatusCode, string? description = null) where T : HttpMethodAttribute
    {
        var methodAttributes = attributes.OfType<T>();
        if(methodAttributes.Any()) {
            var defaultResponseCode = ((int)HttpStatusCode.OK).ToString(CultureInfo.InvariantCulture);
            var responseCode = ((int)httpStatusCode).ToString(CultureInfo.InvariantCulture);
            var responseDescription = description ?? httpStatusCode.ToString();
            operation.Responses.Add(responseCode, new OpenApiResponse {
                Description = responseDescription,
                Content = operation.Responses[defaultResponseCode].Content,
            });
            operation.Responses.Remove(defaultResponseCode);
        }
    }

    private const string json400 = @"{
        ""type"": ""https://tools.ietf.org/html/rfc7231#section-6.5.1"",
        ""title"": ""One or more validation errors occurred."",
        ""status"": 400,
        ""traceId"": ""00-f6332b5860f58b4352834c9fee958825-c5984f3fe6af0a5f-00"",
        ""errors"": {
            ""UUID"": [
                ""The indicated input could not be parsed as a UUID.""
            ]
        }
    }";

    private const string json401 = @"{
        ""type"": ""https://<host>/problems/unauthorized"",
        ""title"": ""You do not have the necessary authorization to access this resource."",
        ""status"": 401,
        ""detail"": ""The resource you're trying to access is restricted. Please ensure you're logged in with the appropriate credentials."",
        ""instance"": ""/api/addresses/21ce71c0-b396-42bf-aa5a-f72a5fafaa3a""
    }";

    private const string json403 = @"{
        ""type"": ""https://<host>/problems/forbidden"",
        ""title"": ""You do not have the necessary permissions to access this resource."",
        ""status"": 403,
        ""detail"": ""Access to this resource is governed by policies that may involve the current state of the entity.  Details on attribute based access conditions can be found in the API Instructions, for endpoint specific details see the endpoint documentation."",
        ""instance"": ""/api/addresses/21ce71c0-b396-42bf-aa5a-f72a5fafaa3a""
    }";

    private const string json404 = @"{
        ""type"": ""https://<host>/problems/argument-out-of-range-exception"",
        ""title"": ""Specified argument was out of the range of valid values. (Parameter 'companyId')"",
        ""status"": 404,
        ""instance"": ""/api/companies/21ce71c0-b396-42bf-aa5a-f72a5fafaa3a""
    }";
}
