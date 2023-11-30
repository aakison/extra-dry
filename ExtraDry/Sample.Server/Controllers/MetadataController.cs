using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Server;
using System.Diagnostics.CodeAnalysis;
using System.Security;

namespace Sample.Server.Controllers;

/// <summary>
/// This controller contains metadata endpoints to test the API.
/// </summary>
[ApiController]
[ApiExceptionStatusCodes]
[ApiExplorerSettings(GroupName = ApiGroupNames.SampleApi, IgnoreApi = false)] 
public class MetadataController {

    /// <summary>
    /// Checks the application service is running without checking database dependencies.
    /// </summary>
    [HttpGet("api/v2/heartbeat")]
    [AllowAnonymous]
    [Produces("application/json")]
    public MessagePayload RetrieveHeartbeat()
    {
        return new MessagePayload();
    }

    /// <summary>
    /// Forces an exception to be thrown to test error handling.
    /// </summary>
    /// <param name="code">A known code for problem details, in set { 400, 403, 404, 500, 501 }.</param>
    [HttpGet("api/v2/exception/{code}")]
    [AllowAnonymous]
    [Produces("application/json")]
    [SuppressMessage("Security", "DRY1106:HttpVerbs methods should not take int values", Justification = "Not an internal ID.")]
    [SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "For testing purposes.")]
    public MessagePayload RetrieveException(int code = 404)
    {
        return code switch {
            400 => throw new ArgumentMismatchException("Bad Request", nameof(code)),
            403 => throw new SecurityException("Not Authorized"),
            404 => throw new ArgumentOutOfRangeException(nameof(code), "Not Found"),
            500 => throw new Exception("Internal Server Error"),
            501 => throw new NotImplementedException("Not Implemented"),
            _ => new MessagePayload { Message = "Unrecognized error code, no exception thrown." }
        };
    }

}

/// <summary>
/// Sample response payload for metadata checks.
/// </summary>
public class MessagePayload {
    /// <summary>
    /// Message to return.
    /// </summary>
    public string Message { get; set; } = "Lub-dub lub-dub lub-dub";
}
