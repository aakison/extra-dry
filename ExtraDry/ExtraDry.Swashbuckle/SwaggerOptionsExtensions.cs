using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ExtraDry.Swashbuckle;

public static class SwaggerOptionsExtensions {

    public static void AddExtraDry(this SwaggerGenOptions openapi)
    {
        openapi.SwaggerDoc(GroupName, new OpenApiInfo {
            Version = "v1",
            Title = "API Instructions",
            Description = "This API provides consistent access to services available on this system conforming to Extra DRY principles.  The following instructions are applied consistently across the entire API set.",
        });

        foreach(var docfile in new string[] { "ExtraDry.Core.Xml", "ExtraDry.Swashbuckle.xml" }) {
            var webAppXml = Path.Combine(AppContext.BaseDirectory, docfile);
            openapi.IncludeXmlComments(webAppXml, includeControllerXmlComments: true);
        }

        openapi.OperationFilter<SignatureImpliesStatusCodes>();
        openapi.OperationFilter<QueryDocumentationOperationFilter>();
        openapi.DocumentFilter<DisplayControllerDocumentFilter>();
        openapi.SchemaFilter<ExtraDrySchemaFilter>();
        openapi.DocumentFilter<ResourceReferenceSchemaFilter>();
    }

    [Obsolete("Use `UseExtraDry` instead.")]
    public static void AddExtraDry(this SwaggerUIOptions swagger)
    {
        swagger.UseExtraDry();
    }

    public static void UseExtraDry(this SwaggerUIOptions swagger)
    {
        swagger.SwaggerEndpoint($"/swagger/{GroupName}/swagger.json", "Instructions");
        swagger.EnableTryItOutByDefault();
        swagger.DocExpansion(DocExpansion.List);
    }

    public static IMvcBuilder AddExtraDry(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddApplicationPart(typeof(SwaggerOptionsExtensions).Assembly);
        return mvcBuilder;
    }

    public const string GroupName = "instructions";

}

