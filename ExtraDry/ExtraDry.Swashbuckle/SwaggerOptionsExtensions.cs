using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ExtraDry.Swashbuckle;

public static class SwaggerOptionsExtensions {

    /// <summary>
    /// Configure and add additional features to the generation of OpenAPI documents aligned with 
    /// features and best practices of Extra Dry.
    /// </summary>
    public static void AddExtraDry(this SwaggerGenOptions openapi, Action<ExtraDryGenOptions>? configure = null)
    {
        var options = new ExtraDryGenOptions();
        configure?.Invoke(options);
        
        if(options.Instructions.Include) {
            openapi.SwaggerDoc(GroupName, new OpenApiInfo {
                Version = options.Instructions.Version,
                Title = options.Instructions.Title,
                Description = options.Instructions.Description,
            });
        }

        if(options.XmlComments.IncludeExtraDryDocuments) {
            foreach(var docfile in new string[] { "ExtraDry.Core.Xml", "ExtraDry.Swashbuckle.xml" }) {
                var webAppXml = Path.Combine(AppContext.BaseDirectory, docfile);
                openapi.IncludeXmlComments(webAppXml, includeControllerXmlComments: true);
            }
        }
        foreach(var docfile in options.XmlComments.Files) {
            var webAppXml = Path.Combine(AppContext.BaseDirectory, docfile);
            openapi.IncludeXmlComments(webAppXml, includeControllerXmlComments: true);
        }

        if(options.Filters.EnableSignatureStatusCodes) {
            openapi.OperationFilter<SignatureImpliesStatusCodes>();
        }
        if(options.Filters.EnableQueryDocumentation) {
            openapi.OperationFilter<QueryDocumentationOperationFilter>();
        }
        if(options.Filters.EnableReadOnlyOnSchema) {
            openapi.SchemaFilter<ExtraDrySchemaFilter>();
        }
        if(options.Filters.EnableResourceReferenceSchemaRewrite) {
            openapi.DocumentFilter<ResourceReferenceSchemaFilter>();
        }
        if(options.Filters.EnableDisplayOnApiControllers) {
            openapi.DocumentFilter<DisplayControllerDocumentFilter>();
        }
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

