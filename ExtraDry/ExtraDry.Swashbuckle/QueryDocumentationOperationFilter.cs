using ExtraDry.Server.Internal;

namespace ExtraDry.Swashbuckle;

/// <summary>
/// During construction of the SwaggerGen, use the Attributes to intuit the likely HTTP response error codes.
/// </summary>
public class QueryDocumentationOperationFilter : IOperationFilter {

    /// <summary>
    /// Scan through each operation, using attribute signatures to guess the typical client errors that will be surfaced.
    /// </summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var takesFilter = context.MethodInfo.GetParameters()
            .Any(e => typeof(FilterQuery).IsAssignableFrom(e.ParameterType));
        var returnType = context.MethodInfo.ReturnType;
        // Strip Task, List, ICollection, FilteredCollection, etc.
        while(returnType.IsGenericType) {
            returnType = returnType.GenericTypeArguments.First();
        }

        if(takesFilter) {
            var modelDescription = new ModelDescription(returnType);

            operation.Description += FilterDescription(modelDescription.FilterProperties.ToArray());
            operation.Description += SortDescription(modelDescription.SortProperties.ToArray());

            var filterableQuotedNames = modelDescription.FilterProperties.Select(e => $"`{e.ExternalName}`");
            var filterable = string.Join(", ", filterableQuotedNames);
            var filterParam = operation.Parameters.FirstOrDefault(e => e.Name == "Filter");
            if(filterParam != null) {
                filterParam.Description += $" Filter fields include any of [{filterable}]";
            }

            var sortableQuotedNames = modelDescription.SortProperties.Select(e => $"`{e.ExternalName}`");
            var sortable = string.Join(", ", sortableQuotedNames);
            var sortParam = operation.Parameters.FirstOrDefault(e => e.Name == "Sort");
            if(sortParam != null) {
                sortParam.Description += $" Sort field is one of [{sortable}]";
                sortParam.Schema.Enum = ArrayOfString(modelDescription.FilterProperties.Select(e => e.ExternalName));
            }
        }
    }

    private static OpenApiArray ArrayOfString(IEnumerable<string> values)
    {
        var openApiValues = new OpenApiArray();
        foreach(var value in values) {
            openApiValues.Add(new OpenApiString(value));
        }
        return openApiValues;
    }

    private static string SortDescription(SortProperty[] sortProps)
    {
        var sortableQuotedNames = sortProps.Select(e => $"  * `{e.ExternalName}`\r\n");
        var sortable = string.Join("", sortableQuotedNames);
        var description = $@"
## Sorting
The `sort` and `ascending` parameters allow for the sorting of the results before being returned.

### Sortable Fields (endpoint specific)
The sort parameter, if provided, is only valid for specific sortable fields, this is one of:
{sortable}

The ascending parameter may take the value `ascending` or `descending` to control order, if not provided then `ascending` is the default.
";
        return description;
    }

    private static string FilterDescription(FilterProperty[] filterProps)
    {
        var description = "## Filtering\nThis endpoint supports filtering using the [standard filtering rules](?urls.primaryName=Instructions).  For performance reasons, not all fields are filterable, and string filters might be applied differently.  The filterable fields for this endpoint are: \n";
        foreach(var filterProp in filterProps) {
            description += $"  * `{filterProp.ExternalName}` ";
            if(filterProp.Property.PropertyType == typeof(string)) {
                description += filterProp.Filter.Type switch {
                    FilterType.Contains => "string field matches term anywhere in string (contains)\r\n",
                    FilterType.StartsWith => "string field matches term at start of string (starts-with)\r\n",
                    _ => "string field matches the entire term (equals)\r\n",
                };
            }
            else if(filterProp.Property.PropertyType == typeof(DateTime)) {
                description += "date field matches value or range\r\n";
            }
            else if(filterProp.Property.PropertyType.IsEnum) {
                var enumValues = Enum.GetNames(filterProp.Property.PropertyType).Select(e => $"`{e}`");
                var values = string.Join(", ", enumValues);
                description += $"enum field matches on specific values [{values}]\r\n";
            }
            else {
                description += "numeric field matches value or range\r\n";
            }
        }
        if(!filterProps.Any()) {
            description = "##### This endpoint does not support filtering.  Add [Filter] attribute to filterable fields.";
        }
        return description;
    }
}
