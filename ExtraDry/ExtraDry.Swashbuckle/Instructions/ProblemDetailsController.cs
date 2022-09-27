using ExtraDry.Server;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace ExtraDry.Swashbuckle.Instructions;

/// <summary>
/// TODO:
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = SwaggerOptionsExtensions.GroupName)]
[ApiExceptionStatusCodes]
[Display(Order = 5)]
public class ProblemDetailsController {

    /// <summary>
    /// Puzzling endpoint, but at least it has good error responses.
    /// </summary>
    /// <param name="input">The query parameter to use get the result.</param>
    [HttpGet("api/sample-data/puzzle"), Produces("application/json")]
    [AllowAnonymous]
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

        int answer;
        if(!int.TryParse(request.Answer.ToString(), out answer)) {
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
