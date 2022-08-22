using System.Reflection;

namespace ExtraDry.Swashbuckle;

public class ExtraDrySchemaFilter : ISchemaFilter {

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var rule = context?.MemberInfo?.GetCustomAttribute<RulesAttribute>();
        if(rule?.UpdateAction == RuleAction.Ignore || rule?.UpdateAction == RuleAction.Block) {
            schema.ReadOnly = true;
        }
    }

}
