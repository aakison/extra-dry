using ExtraDry.Server;
using Microsoft.AspNetCore.Authorization;

namespace ExtraDry.Swashbuckle.Instructions;

/// <summary>
/// When listing entities, the common query mechanism also allows for sorted output.  These list 
/// methods accept a `sort` query parameter, allowing for a field to be selected to order the 
/// results.  Additionally, results can either be sorted ascending or descending using the 
/// `ascending` query parameter. 
/// 
/// In the case of ties, there is no secondary sort key.  The primary use-case for sorting is the 
/// presentation of results to users.  While some interfaces attempt to retain secondary and 
/// tertiary sort criteria, this typically provide little benefit to users and even less to 
/// machine interfaces.
/// 
/// However, there is a significant issue with ties during sort.  When ties occur, the order of 
/// entities is still deterministic and guaranteed to be repeatable.  That is, the overall order 
/// of all items returned will be the same for any given combination of `sort` and `ascending`.  
/// While the stability mechanism is internal and may vary, it is typically the chronological 
/// date of entity creation.
/// 
/// Sorting is endpoint specific and only works on 'sortable fields'.  For performance reasons, 
/// not all fields are sortable.  Each endpoint will list the fields that it can be sorted on.  
/// 
/// ### Try It Out
/// Use the endpoint below to test some sorting against a sample database of car make and 
/// models - no authentication required.  All fields in this example are 'sortable fields'.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerOptionsExtensions.GroupName)]
[ApiExceptionStatusCodes]
[Display(Order = 3)]
public class SortingController {

    public SortingController()
    {
        sampleData = new InstructionDataService();
    }

    /// <summary>
    /// Sortable list of all cars
    /// </summary>
    /// <param name="sort">If the request would like sorted results, the name of the property to sort by.</param>
    /// <param name="ascending">Indicates if the results are requested in ascending order by Sort.</param>
    [HttpGet("api/sample-data/sort-cars"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<FilteredCollection<Automobile>> ListFilteredAsync([FromQuery] SortField? sort, bool ascending = false)
    {
        var query = new FilterQuery { Sort = sort.ToString(), Ascending = ascending };
        return await sampleData.ListAsync(query);
    }

    // Quick and dirty way to get the sort field to show a dropdown.
    // On FilterQuery/PageQuery versions this is done by an IOperationFilter
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortField { Make, Model, Year, Market, Description };

    private readonly InstructionDataService sampleData;

}
