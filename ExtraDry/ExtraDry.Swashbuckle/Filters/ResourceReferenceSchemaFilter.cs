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
        RewriteResourceReferenceTypes(swaggerDoc, context);
    }

    private void RewriteResourceReferenceTypes(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        /*
         *  Loop through the schemas rewriting property types to a corresponding resource reference 
         *  type where needed. When adding a resource reference type generate a schema for it, if it 
         *  doesn't already exist. The `GenerateSchema` method adds the schema to the collection by 
         *  default, using a clone of the collection will avoid any errors due to the collection 
         *  being changed.
         */
        var existingSchemas = swaggerDoc.Components.Schemas.ToList();
        foreach (var schema in existingSchemas) { 
            foreach(var property in schema.Value.Properties) {
                var qualifiedName = $"{schema.Key}.{property.Key}";
                if(typeRewrites.TryGetValue(qualifiedName, out var rewriteType)) {
                    schema.Value.Properties[property.Key] = new OpenApiSchema {
                        Reference = new OpenApiReference() {
                            Id = rewriteType,
                            Type = ReferenceType.Schema,
                        },
                    };
                    if(typeSchema.TryGetValue(rewriteType, out var rewrite) && !context.SchemaRepository.Schemas.ContainsKey(rewriteType)) {
                        context.SchemaGenerator.GenerateSchema(rewrite, context.SchemaRepository);
                    }
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

                    var resourceReferenceType = typeof(ResourceReference<>);
                    resourceReferenceType = resourceReferenceType.MakeGenericType(property.PropertyType);
                    var resourceReferenceSchemeaId = DefaultSchemaIdSelector(resourceReferenceType);
                    typeRewrites.Add($"{typeName}.{propertyName}", resourceReferenceSchemeaId);
                    if(!typeSchema.ContainsKey(resourceReferenceSchemeaId)) {
                        typeSchema.Add(resourceReferenceSchemeaId, resourceReferenceType);
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// This method is borrowed from Swashbuckle.AspNetCore.SwaggerGen to 
    /// ensure that the reference names we use are the same as the one's 
    /// created by Swashbuckle. We can't use the original method as it's 
    /// scoped as private.
    /// See: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/8f363f7359cb1cb8fa5de5195ec6d97aefaa16b3/src/Swashbuckle.AspNetCore.SwaggerGen/SchemaGenerator/SchemaGeneratorOptions.cs#L44
    /// </summary>
    private string DefaultSchemaIdSelector(Type modelType)
    {
        if(!modelType.IsConstructedGenericType) return modelType.Name.Replace("[]", "Array");

        var prefix = modelType.GetGenericArguments()
                              .Select(DefaultSchemaIdSelector)
                              .Aggregate((previous, current) => previous + current);

        return prefix + modelType.Name.Split('`').First();
    }

    private readonly Dictionary<string, string> typeRewrites = new(StringComparer.InvariantCultureIgnoreCase);
    private readonly Dictionary<string, Type> typeSchema = new(StringComparer.InvariantCultureIgnoreCase);
}
