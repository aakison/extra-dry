using ExtraDry.Server;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace ExtraDry.Swashbuckle.Instructions;

/// <summary>
/// Sometimes things just don't work out.  And when they don't, it can be frustrating when you don't
/// have enough information to find the problem.  To help determine the source of the problem, this
/// API adopts RFC 7807 (https://www.rfc-editor.org/rfc/rfc7807) and reports Problem Details as a 
/// JSON payload when errors occur.
/// 
/// ### RFC 7807
/// When APIs fail, the system will conform to the Problem Details RFC.  This will return a JSON
/// object with additional information that is intended for users to better understand the source of
/// the failure.
/// 
/// In accordance with the RFC, the following members are returned in the JSON object:
/// 
/// `type`
/// : A URI that uniquely identifies the type of the problem.  This is not designed for display and
/// does not link to a page with further information.
/// 
/// `status`
/// : The HTTP status code associated with the problem, will match the status code in the 
/// HTTP header.
/// 
/// `title`
/// : An English language summary of the problem.  It does not contain specific details of the 
/// problem and does not change from occurrance to occurrance.  E.g. "Entity could not be found"
/// 
/// `detail`
/// : An English language description of the problem.  This expands on the title and might include
/// specific additional dynamic information about the request or the 
///
/// `instance`
/// : The specific instance that caused this problem, as this is a RESTful API, this is the URI path
/// and query of the HTTP request.
///
/// ### Example
/// ```
/// {
///   "type" : "/problems/entity-not-found",
///   "status" : 404,
///   "title" : "Entity could not be found",
///   "detail" : "Could not find 'Company' with the 'companyId' of '123'.",
///   "instance" : "/api/companies/123"
/// }
/// ```
/// 
/// ### Exceptions
/// Sometimes, the system is unable to return Problem Details.  For example, a call might be
/// throttled by a Web Application Firewall (WAF) and return a 429 - Too Many Requests response
/// before this API is called. Alternately, our system (with our deepest regrets) might have an
/// internal failure and return a 500 - Internal Server Error.
///
/// ### Try It Out
/// Use the endpoint below to see an example of problem details.  There is only one input that does
/// not produce a problem details response, and inputs will provide different responses to
/// demonstrate how details should assist the developer or user to get unblocked.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerOptionsExtensions.GroupName)]
[ApiExceptionStatusCodes]
[Display(Name = "Problem Details", Order = 5)]
public class ProblemDetailsController {

    /// <summary>
    /// Puzzling endpoint, but at least it has good error responses.
    /// </summary>
    /// <param name="input">The query parameter to use get the result.</param>
    [HttpGet("api/sample-data/puzzle"), Produces("application/json")]
    [AllowAnonymous]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Standard pattern.")]
    public Response RetrievePuzzleQuestion([FromQuery] string? input)
    {
        if(string.IsNullOrWhiteSpace(input)) {
            // Failure 1 - no input
            throw new DryException(System.Net.HttpStatusCode.BadRequest, "Missing Input", "The input provided did not exist or was just whitespace.");
        }

        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true,
        };
        Request? request;
        try {
            request = JsonSerializer.Deserialize<Request>(input, options);
        }
        catch(JsonException) {
            // Failure 2 - not JSON at all
            throw new DryException(System.Net.HttpStatusCode.BadRequest, "Not Valid JSON", $"The input provided '{input}' did not parse as valid JSON.");
        }

        if(request?.Answer == null) {
            // Failure 3 - JSON doesn't have "answer"
            throw new DryException(System.Net.HttpStatusCode.BadRequest, "JSON Missing Field", $"The input provided '{input}' did not contain a member named 'answer'.");
        }

        if(!int.TryParse(request.Answer.ToString(), out int answer)) {
            // Failure 4 - Answer isn't a number
            throw new DryException(System.Net.HttpStatusCode.BadRequest, "JSON Field Type Mismatch", $"The answer provided '{request?.Answer}' is not a valid integer.");
        }

        if(answer != 233168) {
            // Failure 5 - Not the right answer
            throw new DryException(System.Net.HttpStatusCode.BadRequest, "Answer is Incorrect", $"Expected the answer to https://projecteuler.net/problem=1");
        }

        return new Response();
    }

    public class Request
    {
        public object? Answer { get; set; }
    }

    public class Response {
        /// <summary>
        /// A congratulatory message.
        /// </summary>
        /// <example>Got to solve the puzzle first...</example>
        public string Congratulations { get; set; } = "You are a true hacker.  Please enjoy a moment of Zen contemplation knowing that this achievement cannot be explained to those who don't understand and will not be appreciated by those that do.";
    }

}
