using ExtraDry.Server;
using Microsoft.AspNetCore.Authorization;

namespace ExtraDry.Swashbuckle.Instructions;

/// <summary>
/// This API is exposed using the OpenAPI 3.0 specification.  This provides a rigorous and discoverable set of APIs which can be easily consumed by humans, as well as by machines that make service access layers for application.
/// 
/// For example, this API can be used:
///   * Through a web page interactive interface (you might be reading on this page right now);
///   * As a API collection through products like Postman;
///   * Read in through a code generator like AutoRest to generate a client-side service access layer.
/// 
/// When using this API, note some of the features.
///   * Semantic Versioning - In a major.minor.patch format, where breaking changes are not introduced without changing the major version.
///   * All endpoints are secured, or in some cases explicitly not secured (i.e.the samples).  Before using, authentication tokens will need to be issued.
/// 
/// ### APIs in this collection
/// APIs are divided into logical groupings to make them more accessible, both when learning the APIs and when consuming through code generation tools.For example, these instructions form part of the 'instructions' API.This API doesn't provide more than sample information to get started quickly with the APIs.  The other APIs that are available include:
///   * Sample APIs - contain the core entities for the system.
///   * Reference Codes - contains codes and configuration.
///   The complete list of all API specifications is in the dropdown at the top of the page.  Each specification page will list provide information on the endpoints and the schema, along with a link to the OpenAPI specification document (e.g. [/ swagger / reference - codes / swagger.json])
///   
/// ### RESTful Design
/// All the APIs in this collection follow RESTful principles to REST Maturity Level 2.  In short, expect to see:
///   * HTTP protocol over secure HTTPS connections
///   * Exchange of data using JSON formats
///   * Use of HTTP status codes to indicate success and failure conditions
///   * Endpoints that act on individual or collective resources as entities(not remote procedure calls)
///   * Use of HTTP verbs to indicate common actions on entities, i.e.PUT/GET/POST/DELETE for create/retrieve/update/delete.
///   * Safe/non-destructive operations for data retrieval
///   * Idempotent changes of data - if you send the same change request twice, the final state is the same
///   * APIs use OpenAPI documentation for discoverability (as opposed to HATEOAS defined in REST Maturity Level 3).
///   
/// ### Cross-Cutting Design
/// In order to make these APIs more approachable, endpoints share some common interface details.  For example, sorting of results from collections of entities is always done with the same query request format, and always produces the same metadata container in the result.These design styles cut-across the entire system and are documented here with these instructions.When you're using other APIs, they will always link back to these instructions for review when needed.
/// 
/// The key concerns are:
///   * Filtering - how to reduce the number of results when querying large sets of entities.
///   * Sorting - how to organize lists of entities for consumption, especially when displaying results to users.
///   * Paging - how to efficiently get large sets of data one page at a time, for both display to users and machine-agent batch processing.
///   
/// ### Live Samples
/// Where possible, these instructions also contain live samples to demonstrate these concepts.  For example, if this all feels too much, why not start with this affirmation:
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerOptionsExtensions.GroupName)]
[ApiExceptionStatusCodes]
[Display(Name = "Getting Started", Order = 1)]
public class GettingStartedController {

    public GettingStartedController()
    {
        sampleData = new InstructionDataService();
    }

    /// <summary>
    /// Take a step towards gratitude
    /// </summary>
    [HttpGet("api/sample-data/transient-affirmation"), Produces("application/json")]
    [AllowAnonymous]
    public AffirmationContainer RetrieveAffirmation()
    {
        return new AffirmationContainer { Affirmation = sampleData.RandomAffirmation() };
    }

    public class AffirmationContainer {

        public string? Affirmation { get; set; }

    }

    private readonly InstructionDataService sampleData;

}
