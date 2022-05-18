#nullable enable

using ExtraDry.Core;
using ExtraDry.Server.Internal;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace ExtraDry.Swashbuckle {

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
                    filterParam.Description += $" Fields optionally can be any of [{filterable}]";
                }

                var sortableQuotedNames = modelDescription.SortProperties.Select(e => $"`{e.ExternalName}`");
                var sortable = string.Join(", ", sortableQuotedNames);
                var sortParam = operation.Parameters.FirstOrDefault(e => e.Name == "Sort");
                if(sortParam != null) {
                    sortParam.Description += $" One of [{sortable}]";
                }
            }
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
            var description = @"

## Filtering
The `filter` parameter allows for both simple and advanced filtering scenarios against a pre-defined set of fields.

### Simple Filters
Simple filters contains a space-separated list of terms.  The endpoint will then return all items that match any of the terms on any of the filterable fields.  Quotes are required to enclose spaces and any punctuation, but not for simple words or numbers.  For example, the following are valid simple filters:

  * `Toyota Honda` - returns all matches of 'Toyota' _or_ 'Honda' on any filterable fields.
  * `""Toyota Corolla""` - returns all matches of 'Toyota Corolla' on any filterable fields.
  * `""Toyota Corolla"" ""Honda Civic""` - returns all matches of 'Toyota Corolla' _or_ 'Honda Civic' on any filterable fields.
  * `2011` - returns all strings that contain '2011' and numbers that equal 2011 on any filterable fields.

This simple search is suitable for most use-cases where a filter string is provided to an end-user.

### Advanced Filters
It is also possible to specify specific fields for filter terms.  Provide the field name and a colon before the filter term.  For example, the following are valid advanced filters:

  * `make:Toyota model:Corolla` - returns all matches of 'Toyota' in the 'make' field _and_ 'Corolla' in the 'model' field.
  * `make:Toyota model:""FJ Cruiser""` - returns all matches of 'Toyota' in the 'make' field _and_ 'C-HR' in the 'model' field, note the quotes are required because of the space in the name 'FJ Cruiser'. 

Advanced filters can also specify a list of alternate values.  This can be done by either listing the field multiple times or by separating the terms using a pipe '|' character.  For example, all of the following are equivelent and will return all matches of 'Corolla' in the 'model' field union with all matches of 'Prado' in the 'model' field.

  * `model:Corolla model:Prado`
  * `model:""Corolla"" model:""Prado""`
  * `model:Corolla|Prado`
  * `model:""Corolla""|""Prado""`

### Range Filters
Number and DateTime filterable fields also support ranges.  To specify a range, place the lower and upper bounds inside parantheses and/or brackets, with a comma in between, e.g. `(0,10)` or `[1,4]`.  Either the lower or upper bound (but not both) can be omitted for open ranges.  Parentheses '(' and ')' indicate exclusive boundaries, while brackets '[' and ']' indicate inclusive boundaries.  For example, the following are valid range filters:

  * `year:[2000,2010)` - All years from 2000 up to and exluding 2010.
  * `year:[2010,)` - All years from 2010 onwards (no upper bound).
  * `date:[2020-01-01T00:00:00,2021-01-01T00:00:00)` - Any time in calendar year 2020.
  * `age:(,18)` - Any age below 18.

### Filterable Fields (endpoint specific)
For performance reasons, not all fields are filterable, and string filters might be applied differently.  Strings will match either the whole string, the start of the string, or anywhere in the string.  The filterable fields for this endpoint are:

";
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
            return description;
        }
    }

}
