using ExtraDry.Server;
using Microsoft.AspNetCore.Authorization;

namespace ExtraDry.Swashbuckle.Instructions;

/// <summary>
/// Some data entities naturally occur in a hierarchy which is often represented as a tree. This
/// presents some unique challenges when it comes to querying and displaying the data. For these
/// entities, an additional API endpoint is provided.
///
/// The hierarchy endpoint provides the common interaction mechanisms for working with entities as
/// a tree in addition to treating it as a list. This includes a different query mechanism to
/// collect a tree subset, as well as a guaranteed depth-first ordering which suits row based
/// display to users.
///
/// ### Queries When requesting trees of data, the query mechanism takes levels, nodes to expand,
/// nodes to collapse, and arbitrary filter conditions.
///
/// #### Level Query The level query is the simplest query mechanism and limits the number of
/// levels returned. This can be used alone or in conjunction with other query mechanisms to limit
/// the scope of their return results.
///
/// This can support user interfaces where the entire tree can be expanded and collapsed an entire
/// level at a time.
///
/// E.g. `?level=1` will return the first two levels of animals in the sample below (levels are 0
/// indexed).
///
/// #### Expand Query If provided, this is an array of node identifiers that should be expanded.
/// The transient identifier, Slug, is used for this operation. This will add to the result set all
/// children of the provided nodes.
///
/// This can support user interfaces where individual nodes can be expanded.
///
/// E.g. `?level=1&amp;expand=vertebrates` will expand vertebrates but leave invertebrates
/// collapsed.
///
/// #### Collapse Query If provide, this is an array of node identifiers that should be collapsed.
/// The transient identifier, Slug, is used for this operation. This will remove the children of
/// this node from being displayed in the result set, even if they would otherwise be included in a
/// Level, Expand, or Filter query.
///
/// This can support user interfaces where individual nodes can be collapsed.
///
/// E.g. `?level=2&amp;collapse=invertebrates` will collapse invertabrates but leave vertebrates
/// expanded, return the same results as the example above.
///
/// #### Filter Query The most complex, and most computationally expensive, query mechanism is the
/// filter. This uses the same filter query as the general filter mechanism and is applied to the
/// tree within the number of levels requests. To filter the entire tree, set the Level to a number
/// greater than the depth of the tree.
///
/// When applied with a hierarchy, it is also necessary that the filtered nodes return all their
/// ancestors.
///
/// This can support user interfaces where trees can be filtered and still shown as trees. In many
/// instances, this is confusing for users and using the list query mechanism may be more
/// appropriate and is faster.
///
/// E.g. `?level=100&amp;filter=elephant` will return the 'animal', 'vertebrates', and 'mammal'
/// nodes in addition to the 'elephant' parent node. While `?level=2&amp;filter=elephant` will
/// return nothing since the node is pruned by the level query.
///
/// Note: It is possible that a collapse node will conflict with the ancestors of a filter node, in
/// these cases the node will be included and the collapse query will be ignored. /// ### Response
/// Body The response body is a container of both items and metadata about the request. In
/// particular, it contains:
/// * `created` - a ISO8601 formatted date with the UTC time the query was run.
/// * `filter` - (optional) if a filter was provided for the query, the text of the filter as
/// requested.
/// * `count` - the number of items returned (can be calculated, but reiterated for convenience).
/// * `items` - an array of entities, serialization varies by endpoint.
/// * `level` - the requested level limit from the query.
/// * `expand` - (optional) the requested nodes to expand from the query.
/// * `collapse` - (optional) the requested nodes to collapse from the query.
///
/// ### Try It Out Use the endpoint below to test some hierarchy queries against a sample database
/// taken from the animal kingdom - no authentication required.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerOptionsExtensions.GroupName)]
[ApiExceptionStatusCodes]
[Display(Name = "Hierarchy Data", Order = 5)]
public class HierarchyController
{
    public HierarchyController()
    {
        sampleData = new InstructionDataService();
    }

    /// <summary>
    /// List of animals in hierarchy order.
    /// </summary>
    [HttpGet("api/sample-data/region-hierarchy"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<HierarchyCollection<Animal>> ListHierarchyAsync([FromQuery] HierarchyQuery query)
    {
        return await sampleData.ListHierarchyAsync(query);
    }

    private readonly InstructionDataService sampleData;
}
