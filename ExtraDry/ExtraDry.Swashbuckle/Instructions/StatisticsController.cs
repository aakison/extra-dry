using ExtraDry.Server;
using Microsoft.AspNetCore.Authorization;

namespace ExtraDry.Swashbuckle.Instructions;

/// <summary>
/// Some components will also provide a statistics endpoint.  This is designed to quickly provide
/// some basic rollup information about the entities.  It is not designed as a full reporting or 
/// Data Warehouse solution.
/// 
/// The information is provided to assist in some quick answers to common questions.  For
/// example, in a ticketing system it would be desirable to show users the count of tickets marked
/// as 'critical'.  Or the number of closed tickets in the past 30 days.
/// 
/// The statistics endpoints will accept a filter query (see above), which first filters all 
/// entities before calculating the statistics.
/// 
/// Statistics are endpoint specific and when available only work on 'statistics fields'.  For 
/// performance and logic reasons, not all fields report statistics.  For example, a text 
/// description is unlikely to have any meaning if statistical analysis was to be performed.  
/// 
/// ### Types of Statistics
/// 
///   * *Distribution* - For fields that contain a small set of discrete values, the set of 
///     distinct values is returned along with the number of occurrences of that value.
/// 
/// ### Try It Out
/// Use the endpoint below to retrieve the statistics for the sample database of car make and 
/// models - no authentication required.  
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerOptionsExtensions.GroupName)]
[ApiExceptionStatusCodes]
[Display(Order = 4)]
public class StatisticsController {

    public StatisticsController()
    {
        sampleData = new InstructionDataService();
    }

    /// <summary>
    /// Statistics about some cars.
    /// </summary>
    /// <param name="query">The filter to apply to the collection prior to collecting statistics.</param>
    [HttpGet("api/sample-data/stats"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<Statistics<Automobile>> RetrieveStatsAsync([FromQuery] FilterQuery query)
    {
        return await sampleData.RetrieveStatsAsync(query);
    }

    private readonly InstructionDataService sampleData;

}
