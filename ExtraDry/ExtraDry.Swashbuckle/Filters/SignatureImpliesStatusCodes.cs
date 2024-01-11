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

        var attributes = context.MethodInfo.GetCustomAttributes(true) ?? Array.Empty<object>();

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
            ""type"": ""t"",
            ""title"": ""title"", 
            ""status"": 404,
            ""detail"": ""The requested resource was not found, no entity with indicated UUID exists."",
            ""instance"": ""localhost""
        }";
}
