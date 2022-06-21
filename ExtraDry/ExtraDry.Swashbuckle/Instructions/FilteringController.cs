using Microsoft.AspNetCore.Authorization;

namespace Sample.Swashbuckle.Instructions;

/// <summary>
/// Most API endpoints have the ability to list entities.  When provided, these list methods accept a `filter` query parameter, allowing for filtering results to a subset of entities.
/// 
/// The `filter` parameter allows for both simple and advanced filtering scenarios against a pre-defined set of fields.  The simple version of the filters are suitable for filters that might be seen by average users.  While the advanced filters start to look more complex than users prefer.  The advanced options, however, are suitable for constructing filter view for users with multiple UI controls.
/// 
/// ### Simple Filters
/// Simple filters contain just a space-separated list of terms.  The filter will return all items that match any of the terms on any of the filterable fields.  
/// For example:
/// 
/// * `Toyota Honda` - returns all matches of 'Toyota' _or_ 'Honda' on any filterable fields.
/// * `Toyota Civic` - returns all Toyotas and the Civic from Honda.  Also, anything that has a description that compares itself to a Civic.
/// * `2011` - returns all strings that contain '2011' and numbers that equal 2011 on any filterable fields.
/// 
/// To filter based on phrases, use quotation markers to enclose spaces and any punctuation.  
/// For example:
/// 
/// * `"Toyota Corolla"` - returns all matches of 'Toyota Corolla' on any filterable fields.
/// * `"Toyota Corolla" "Honda Civic"` - returns all matches of 'Toyota Corolla' _or_ 'Honda Civic' on any filterable fields.
/// 
/// ### Advanced Filters
/// Using advanced filters, it is also possible to specify fields for filter terms.  Simply provide the field name and a colon before the filter term.
/// For example:
/// 
/// * `make:Toyota model:Corolla` - returns all matches of 'Toyota' in the 'make' field _and_ 'Corolla' in the 'model' field.
/// * `make:Toyota model:"FJ Cruiser"` - returns all matches of 'Toyota' in the 'make' field _and_ 'FJ Cruiser' in the 'model' field, note the use of quotes because of the space in the name 'FJ Cruiser'. 
/// 
/// Advanced filters can also specify a list of alternate values.  This can be done two ways.  Either listing the field multiple times or by separating the terms using a pipe '|' character.  
/// 
/// For example, all of the following are equivalent and will return all matches of 'Corolla' in the 'model' field union with all matches of 'Prado' in the 'model' field.
/// 
/// * `model:Corolla model:Prado`
/// * `model:"Corolla" model:"Prado"`
/// * `model:Corolla|Prado`
/// * `model:"Corolla"|"Prado"`
/// 
/// ### String Constraints
/// When searching fields for string values, the API will decide which matching logic to apply.  This will be one of the following:
/// * Match the full field only (equal-to) - This will only match if the text requested matches the entity exactly.  This is common for status fields. E.g. a search for 'active' should not match a status of 'inactive'.
/// * Match the beginning of the field (starts-with) - Will match any string that begins with the text requested.  Less common, this can be used effectively for surnames.
/// * Match anywhere within the field (contains) - Will match any part of the string.  This is common for searching in titles and descriptions but is also offers the lowest performance.
/// 
/// When calling the API, you cannot request a constraint type.  Instead, each API is pre-configured to treat each field with the appropriate constraint.  This ensures requests remain simple and provide expected results.  The filter documentation for each endpoint describes which constraint is applied to which field.
/// 
/// ### Range Filters
/// Number and DateTime filterable fields also support ranges.
/// To specify a range, comma separate the lower and upper bounds inside parantheses and/or brackets, e.g. `(0,10)` or `[1,4]`.  
/// Parentheses '(' and ')' indicate exclusive boundaries, while brackets '[' and ']' indicate inclusive boundaries.
/// Either the lower or upper bound (but not both) can be omitted for open ranges.
/// For example, the following are valid range filters:
/// 
/// * `year:[2000,2010)` - All years from 2000 up to and exluding 2010.
/// * `year:[2010,)` - All years from 2010 onwards (no upper bound).
/// * `date:[2020-01-01T00:00:00,2021-01-01T00:00:00)` - Any time in calendar year 2020.
/// * `age:(,18)` - Any age below 18.
/// 
/// ### Filterable Fields
/// For performance reasons, not all fields are filterable.  So each of the above examples only works on 'filterable fields'.  Individual endpoints will list the fields that can be filtered on.  
///
/// ### Response Body
/// The response body is a container of both items and metadata about the request.  In particular, it contains:
/// * `created` - a ISO8601 formatted date with the UTC time the query was run.
/// * `filter` - (optional) if a filter was provided for the query, the text of the filter as requested.
/// * `count` - the number of items returned (can be calculated, but reiterated for convenience).
/// * `items` - an array of entities, serialization varies by endpoint. 
/// 
/// ### Try It Out
/// Use the endpoint below to test some filters against a sample database of car make and models - no authentication required.  All fields in this example are 'filterable fields'.
/// Try the following:
/// * *blank* - get a full list of all cars, no filter applied.
/// * `Toyota` - get a list of vehicles that reference 'Toyota'.
/// * `make:toyota` - get a list of just vehicles that _are_ a toyota.
/// * `make:toy` - empty list, the 'make' field is a full match, example of 'equality' filter.
/// * `model:yaris` - only returns the 'Yaris Cross' and not the 'GT Yaris', example of 'starts-with' filter.
/// * `year:[1960,1990)` - see only the oldest cars.
/// * Look at the JSON produced and mix and match, all automobile fields are filterable.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerOptionsExtensions.GroupName)]
[SkipStatusCodePages]
[Display(Order = 2)]
public class FilteringController {

    public FilteringController()
    {
        sampleData = new InstructionDataService();
    }

    /// <summary>
    /// Filterable list of all cars
    /// </summary>
    /// <param name="filter">The entity specific text filter for the collection. Filter fields include any of [`Make`, `Model`, `Year`, `Market`, `Description`]</param>
    [HttpGet("sample-data/filter-cars"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<FilteredCollection<Automobile>> ListFilteredAsync([FromQuery] string? filter)
    {
        var query = new FilterQuery { Filter = filter };
        return await sampleData.ListAsync(query);
    }

    private readonly InstructionDataService sampleData;

}
