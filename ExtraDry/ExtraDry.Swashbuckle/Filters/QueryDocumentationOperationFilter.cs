using ExtraDry.Server.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
        var supportsFiltering = context.MethodInfo.GetParameters()
            .Any(e => typeof(FilterQuery).IsAssignableFrom(e.ParameterType));
        var supportsSorting = context.MethodInfo.GetParameters()
            .Any(e => typeof(SortQuery).IsAssignableFrom(e.ParameterType));
        var supportsPaging = context.MethodInfo.GetParameters()
            .Any(e => typeof(PageQuery).IsAssignableFrom(e.ParameterType));

        var returnType = context.MethodInfo.ReturnType;

        // Strip Task from return type before applying filter.
        if(returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>)) {
            returnType = returnType.GenericTypeArguments.First();
        }

        var supportsStatistics = returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Statistics<>);

        // Strip Statistics<>, List<>, ICollection<>, FilteredCollection<>, etc.
        while(returnType.IsGenericType) {
            returnType = returnType.GenericTypeArguments.First();
        }

        var modelDescription = new ModelDescription(returnType);
        if(supportsFiltering) {
            operation.Description += FilterDescription(modelDescription.FilterProperties.ToArray());

            var filterableQuotedNames = modelDescription.FilterProperties.Select(e => $"`{e.ExternalName}`");
            var filterable = string.Join(", ", filterableQuotedNames);
            var filterParam = operation.Parameters.FirstOrDefault(e => e.Name == "Filter");
            if(filterParam != null) {
                filterParam.Description += $" Filter fields include any of [{filterable}]";
            }
        }
        if(supportsSorting) {
            operation.Description += SortDescription(modelDescription.SortProperties.ToArray());

            var sortableQuotedNames = modelDescription.SortProperties.Select(e => $"`{e.ExternalName}`");
            var sortable = string.Join(", ", sortableQuotedNames);
            var sortParam = operation.Parameters.FirstOrDefault(e => e.Name == "Sort");
            var values = modelDescription.SortProperties.Select(e => $"+{e.ExternalName}").Union(modelDescription.SortProperties.Select(e => $"-{e.ExternalName}"));
            if(sortParam != null) {
                sortParam.Description += $" Sort field is one of [{sortable}]";
                sortParam.Schema.Enum = ArrayOfString(values);
            }
        }
        if(supportsStatistics) {
            operation.Description += StatisticsDescription(modelDescription.StatisticsProperties.ToArray());
        }
        if(supportsPaging) {
            operation.Description += PagingDescription();
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

    private static string PagingDescription()
    {
        var description = $@"
## Paging
Paging is supported using the [standard paging rules](?urls.primaryName=Instructions).";
        return description;
    }

    private static string SortDescription(SortProperty[] sortProps)
    {
        var description = $@"
## Sorting
Sorting is supported using the [standard sorting rules](?urls.primaryName=Instructions).  The sortable fields for this endpoint are [{DisplaySortProps()}]";

        if(!sortProps.Any()) {
            description = "##### This endpoint does not support sorting.  Add [Sort] attribute to sortable properties.";
        }

        return description;

        string DisplaySortProps() => string.Join(", ", sortProps.Select(e => $"`{e.ExternalName}`"));
    }

    private static string StatisticsDescription(StatisticsProperty[] statsProps)
    {
        var description = @"
## Statistics
Statistics are returned from this endpoint using the [standard statistics format](?urls.primarName=Instructions).  The statistics returned from this endpoint are:
";
        foreach(var prop in statsProps) {
            description += $"  * `{prop.ExternalName}` ";
            description += prop.Stats switch {
                Stats.Distribution => "discrete values with frequency distribution\r\n",
                _ => "."
            };
        }
        if(!statsProps.Any()) {
            description = "##### This endpoint does not support statistics.  Add [Statistics(...)] attribute on statistics properties.";
        }
        return description;
    }

    private static string FilterDescription(FilterProperty[] filterProps)
    {
        var description = "## Filtering\nFiltering is supported using the [standard filtering rules](?urls.primaryName=Instructions).  The filterable fields for this endpoint are: \n";
        foreach(var filterProp in filterProps) {
            description += $"  * `{filterProp.ExternalName}` ";
            if(filterProp.Property.PropertyType == typeof(string)) {
                description += filterProp.Filter.Type switch {
                    FilterType.Contains => "string field matches term anywhere in string (contains)\r\n",
                    FilterType.StartsWith => "string field matches term at start of string (starts-with)\r\n",
                    _ => "string field that matches the entire term (equal-to)\r\n",
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
