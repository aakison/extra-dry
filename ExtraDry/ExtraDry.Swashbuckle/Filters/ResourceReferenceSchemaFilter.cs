using System.Reflection;

namespace ExtraDry.Swashbuckle;

/// <summary>
/// During construction of the SwaggerGen, use the JsonConverter types that indicate that a 
/// ResourceReference is to be returned to change the type exposed through the API.
/// </summary>
/// <remarks>
/// Works with ResourceReferenceConverter`T to change the serialized type of the entity.
/// </remarks>
public class ResourceReferenceSchemaFilter : IDocumentFilter {

    /// <inheritdoc cref="IDocumentFilter.Apply(OpenApiDocument, DocumentFilterContext)" />
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        LoadResourceReferences();
        RewriteResourceReferenceTypes(swaggerDoc);
    }

    private void RewriteResourceReferenceTypes(OpenApiDocument swaggerDoc)
    {
        foreach(var schema in swaggerDoc.Components.Schemas) {
            foreach(var property in schema.Value.Properties) {
                var qualifiedName = $"{schema.Key}.{property.Key}";
                if(typeRewrites.TryGetValue(qualifiedName, out var rewriteType)) {
                    schema.Value.Properties[property.Key] = new OpenApiSchema {
                        Reference = new OpenApiReference() {
                            Id = rewriteType,
                            Type = ReferenceType.Schema,
                        },
                    };
                }
            }
        }
    }

    private void LoadResourceReferences()
    {
        typeRewrites.Clear();
        foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
            if(assembly.IsDynamic) {
                continue;
            }
            foreach(var type in assembly.ExportedTypes) {
                var properties = type.GetProperties();
                foreach(var property in properties) {
                    var converter = property.GetCustomAttribute<JsonConverterAttribute>();
                    if(converter?.ConverterType == null) {
                        continue;
                    }
                    if(converter.ConverterType.IsGenericType == false) {
                        continue;
                    }
                    if(converter.ConverterType.GetGenericTypeDefinition() != typeof(ResourceReferenceConverter<>)) {
                        continue;
                    }
                    var typeName = type.Name;
                    var propertyName = property.Name;
                    var targetTypeName = converter.ConverterType.GenericTypeArguments.First().Name;
                    var propertyType = $"{targetTypeName}ResourceReference";
                    typeRewrites.Add($"{typeName}.{propertyName}", propertyType);
                }
            }
        }
    }

    private readonly Dictionary<string, string> typeRewrites = new(StringComparer.InvariantCultureIgnoreCase);

}

