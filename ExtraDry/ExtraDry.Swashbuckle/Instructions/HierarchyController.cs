using ExtraDry.Server;

namespace ExtraDry.Swashbuckle.Instructions;

/// <summary>
/// When listing entities, sometimes the number of entities returned is too large for a single API
/// call.  This could be because of size limits.  It could also be because of user constraints.
/// Regardless, when APIs may return large sets, they'll usually have a paging mechanism.  
///
/// When paging is required, it always comes in two flavors.  A skip-take variant and a continuation
/// token variant.
/// 
/// ### Skip-Take Pagination
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
/// ### Continuation Token Pagination
/// Slightly more complicated but surprisingly easy to use is the continuation token.  When wanting
/// to process through all entities, as is typical for machine interfaces, request the first page of
/// entities from the API.  In the response, there will be a `continuationToken`.  Send that token
/// back in the next call and it will accurately and methodically page you through all results.
///
/// When using this mechanism, the `sort` and `ascending` properties may be set on the initial API
/// call.  However, they are ignored on subsequent calls, letting the continuation token keep track
/// of them and ensuring consistent results.
///
/// Using a `take` value on the first call will set the page size for all future calls.  Unlike
/// sorting fields though, `skip` and `take` can be used with a token to reset its location.  This
/// is an uncommon scenario.
///
/// To reset the continuation token, just make another call without providing the previous token.
/// 
/// ### Note on Completeness
/// In both the scenarios above, it is possible that entities could be skipped.  In particular when
/// collections have any form of sorting applied.  Imagine the case where user entities are sorted
/// by first name and there are 25 users with a first name starting with 'A'.  On a page of 25
/// results, they all fit nicely in the page.  The expected result at the top of the second page
/// would be the 'B's from "Barbara" to "Bart" to "Bert" and so on.  Next, imagine that while the
/// users are looking at the page of 'A's someone else deletes 'Adam'.  When they skip to the next
/// page the first name will be 'Bart' because 'Barbara' moved to page 1.
///
/// A similar mechanism works in reverse.  When a user is added in the above scenario, they display
/// would duplicate a name on a page.
///
/// As users page back and forth, they'll get caught up and fairly readily understand the issue.
/// However, for machine interfaces that want to process 100% of records, this is not suitable.  To
/// facilitate this case, the only supported mechanism is to use continuation tokens without any
/// sorting mechanism.  This will ensure that the most recent records added are on the last page
/// processed.
/// 
/// ### Response Body
/// The response body for paged results is a superset of filtered results.  In addition to the
/// filter and sorting fields, it also returns:
///   * `start` - the index of the first result amongst all requested entities.
///   * `total` - the total number of entities that can be returned.
///   * `continuationToken` - a string value to be passed back to the API.
/// 
/// Note that the total number might change during processing based on background processes adding
/// or removing entities.
///  
/// ### Try It Out
/// Use the endpoint below to test some paging against a sample database of car make and models - no
/// authentication required.  The sample set is small, so the server limits the maximum `take` size
/// to 10 records.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerOptionsExtensions.GroupName)]
[ApiExceptionStatusCodes]
[Display(Name = "Hierarchy Data", Order = 4)]
public class HierarchyController {

    public HierarchyController()
    {
        sampleData = new InstructionDataService();
    }

    ///// <summary>
    ///// List of regions in hierarchy order.
    ///// </summary>
    //[HttpPost("api/sample-data/region-hierarchy"), Produces("application/json")]
    //[AllowAnonymous]
    //public async Task<PagedCollection<Automobile>> ListAsync(HierarchyQuery query)
    //{
    //    take = take <= 0 ? 10 : Math.Min(take, 10);
    //    var query = new PageQuery { Skip = skip, Take = take, Token = token };
    //    return await sampleData.ListAsync(query);
    //}

    private readonly InstructionDataService sampleData;

}
