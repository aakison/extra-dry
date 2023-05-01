using System.Reflection;

namespace ExtraDry.Swashbuckle;

/// <summary>
/// Updates the OpenAPI schema to apply ReadOnly to properties that can't have data posted back for
/// either a Create or an Update.  Automatically applied when using `openapi.AddExtraDry()` inside
/// of the `AddSwaggerGen` call in Startup.
/// </summary>
/// <remarks>
/// Options for schema are ReadOnly or WriteOnly.  Determines if Swagger UI shows values on 
/// retrieve (WriteOnly = false), or as available for POST/PUT (ReadOnly = false).
/// 
/// Nothing in the system would currently indicate a use for WriteOnly.  
/// 
/// ReadOnly set to true does make sense for fields such as Audit and Revision fields. However, can
/// be mistakenly applied to other unchanging fields such as Tenant ID or UUID.
/// 
/// If a property is Ignored or Blocked for _both_ Create and Update, then it is by definition
/// ReadOnly.  Any other combinations allow for a change sometimes so can't be ReadOnly.
/// 
/// Aggregated objects in OpenApi need to have their ReadOnly flag marked on the schema, so if the
/// MemberInfo is null, then check if the type has a Class level RulesAttribute.
/// </remarks>
public class ExtraDrySchemaFilter : ISchemaFilter {

    /// <inheritdoc cref="ISchemaFilter.Apply(OpenApiSchema, SchemaFilterContext)" />
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var rule = context?.MemberInfo?.GetCustomAttribute<RulesAttribute>() 
            ?? context?.Type?.GetCustomAttribute<RulesAttribute>();
        if((rule?.CreateAction == RuleAction.Ignore || rule?.CreateAction == RuleAction.Block) && 
            (rule?.UpdateAction == RuleAction.Ignore || rule?.UpdateAction == RuleAction.Block)) {
            schema.ReadOnly = true;
        }
    }

}
