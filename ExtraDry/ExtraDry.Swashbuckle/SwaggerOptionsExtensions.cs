using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ExtraDry.Swashbuckle;

public static class SwaggerOptionsExtensions {

    public static void AddExtraDry(this SwaggerGenOptions openapi)
    {
        openapi.SwaggerDoc(GroupName, new OpenApiInfo {
            Version = "v1",
            Title = "Sample API Instructions",
            Description = "This API provides consistent access to services available on this system conforming with Extra DRY principles.",
        });

        foreach(var docfile in new string[] { "ExtraDry.Core.Xml", "ExtraDry.Swashbuckle.xml" }) {
            var webAppXml = Path.Combine(AppContext.BaseDirectory, docfile);
            openapi.IncludeXmlComments(webAppXml, includeControllerXmlComments: true);
        }

        openapi.OperationFilter<SignatureImpliesStatusCodes>();
        openapi.OperationFilter<QueryDocumentationOperationFilter>();
        openapi.DocumentFilter<DisplayControllerDocumentFilter>();
    }

    public static void AddExtraDry(this SwaggerUIOptions swagger)
    {
        swagger.SwaggerEndpoint($"/swagger/{GroupName}/swagger.json", "Instructions");
        swagger.EnableTryItOutByDefault();
        swagger.DocExpansion(DocExpansion.List);
    }

    public const string GroupName = "instructions";

}

