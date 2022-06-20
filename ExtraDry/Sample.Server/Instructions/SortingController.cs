using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Server.Instructions;

/// <summary>
/// When listing entities, the common query mechanism also allows for sorted output.  These list methods accept a `sort` query parameter, allowing for a field to be selected to list output results.  
/// for filtering to only a subset of entities. The `sort` and `ascending` parameters allow for the sorting of the results before being returned.
///
/// Additionally, results can either be sorted ascending or descending using the Sortable fields are endpoint specific, and only specific 
/// 
/// Sorting is endpoint specific and only works on 'filterable fields'.  For performance reasons, not all fields are sortable.  Each endpoint will list the fields that can be sorted on.  
/// 
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiGroupNames.Instructions)]
[SkipStatusCodePages]
public class SortingController {

    public SortingController()
    {
        sampleData = new InstructionDataService();
    }

    /// <summary>
    /// Sortable list of all cars
    /// </summary>
    [HttpGet("sample-data/sort-cars"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<FilteredCollection<Automobile>> ListFilteredAsync([FromQuery] string? sort, bool ascending)
    {
        var query = new FilterQuery() { Sort = sort, Ascending = ascending };
        return await sampleData.ListAsync(query);
    }

    private readonly InstructionDataService sampleData;

}
