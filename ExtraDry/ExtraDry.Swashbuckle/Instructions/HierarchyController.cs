using ExtraDry.Server;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Swashbuckle.Instructions;

/// <summary>
/// TODO: Intro:
/// When listing entities, sometimes the number of entities returned is too large for a single API
/// call.  This could be because of size limits.  It could also be because of user constraints.
/// Regardless, when APIs may return large sets, they'll usually have a paging mechanism.  
///
/// When paging is required, it always comes in two flavors.  A skip-take variant and a continuation
/// token variant.
/// 
/// ### Capabilities
/// * Level only
/// * Level plus expand
/// * Level plus collapse
/// * Any number, so gets big and endpoints likely to be POST
/// * Filter respects level, includes ancestors
/// * Ancestors overrides collapse
/// * Often will support paging, but not tokens.
/// 
/// The conceptually simpler mechanism is that of a skip-take.  You may `skip` as many records as
/// you want and then request to `take` as many as works for your interface.  This mechanism is more
/// typically used when the result of the query is being displayed to a user.  Users can only see
/// and process a limited view, so it makes sense to track where they are just show them a changing
/// subset.
///
/// For a page-based interface, set your `take` to the size of your page.  Then, set your `skip` to
/// the index of the first record on that page.  For example, if you're displaying 25 records per
/// page and are showing page 3, then set `skip` to `75` and `take` to `25`.
///
/// For a continuous-scroll interface, load in the first page of records and take note of the
/// `total` number of entities.  Then, set the size of the scrollbar to the `total`.  Figure out how
/// many records fit on a screen and set this to the window of the scrollbar and your `take`
/// setting.  As the user scrolls, re-request the visible page by sending the index of the scrollbar
/// as the `skip`.  Of course, this glosses over some details and an efficient implementation will
/// load records before and after the current window as well as some sort of client-side caching
/// mechanism.
///
/// This mechanism, however, has some drawbacks.  For example, keeping track of which records you've
/// accessed if you're trying to access all records.  Note also, the API does not guarantee that the
/// number of records you request in `take` will be returned.
/// 
/// ### Response Body
/// TODO:
///  
/// ### Try It Out
/// TODO:
/// Use the endpoint below to test some paging against a sample database of car make and models - no
/// authentication required.  The sample set is small, so the server limits the maximum `take` size
/// to 10 records.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerOptionsExtensions.GroupName)]
[ApiExceptionStatusCodes]
[Display(Name = "Hierarchy Data", Order = 5)]
public class HierarchyController {

    public HierarchyController()
    {
        sampleData = new InstructionDataService();
    }

    /// <summary>
    /// List of animals in hierarchy order.
    /// </summary>
    [HttpGet("api/sample-data/region-hierarchy"), Produces("application/json")]
    [AllowAnonymous]
    [SuppressMessage("Usage", "DRY1104:Http Verbs should be named with their CRUD counterparts", Justification = "Hierarchy queries are an exception.")]
    public async Task<HierarchyCollection<Animal>> ListHierarchyAsync([FromQuery] HierarchyQuery query)
    {
        return await sampleData.ListHierarchyAsync(query);
    }

    private readonly InstructionDataService sampleData;

}
