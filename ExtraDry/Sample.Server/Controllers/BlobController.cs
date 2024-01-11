using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace Sample.Server.Controllers;

/// <summary>
/// Manages blob, such as used in Contents.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiGroupNames.SampleApi)]
[ApiExceptionStatusCodes]
[SuppressMessage("Usage", "DRY1002:ApiController shouldn't inherit from ControllerBase",
    Justification = "Controller makes use of ControllerBase functionality for emitting file content.")]
public class BlobController : ControllerBase
{

    /// <summary>
    /// Standard DI Constructor
    /// </summary>
    public BlobController(InMemoryBlobService blobService)
    {
        blobs = blobService;
    }

    /// <summary>
    /// Creates a Blob with both the given unique key and the user-friendly filename.  
    /// </summary>
    [HttpPost("/api/blobs/{uuid}/{filename}")]
    [Produces("application/json")]
    [AllowAnonymous]
    [SuppressMessage("ApiUsage", "DRY1107:HttpPost, HttpPut and HttpPatch methods should have Consumes attribute", Justification = "Blobs could be any mime type.")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Ignored but included for consistent consumer experience.")]
    public async Task<ResourceReference<Blob>> CreateBlobAsync(Guid uuid, string filename = "")
    {
        var headerUuidStr = Request.Headers[Blob.UuidHeaderName].FirstOrDefault() ?? "";
        if(Guid.TryParse(headerUuidStr, out var headerUuid)) {
            if(headerUuid != uuid) {
                throw new ArgumentMismatchException("UUID in header does not match UUID in URL", nameof(uuid));
            }
        }

        var exemplar = await BlobSerializer.DeserializeBlobAsync<Blob>(Request);

        var blob = await blobs.CreateAsync(exemplar);
        return new ResourceReference<Blob>(blob);
    }

    /// <summary>
    /// Retrieves a Blob with the specified UUID as a file stream and HTTP headers.
    /// </summary>
    [HttpGet("api/blobs/{uuid}")]
    [HttpGet("api/blobs/{uuid}/{filename}")]
    [AllowAnonymous]
    [SuppressMessage("ApiUsage", "DRY1109:HttpGet methods should have Produces attribute", Justification = "May produce various content types, set using the FileContentResult.")]
    public async Task RetrieveAsync(Guid uuid, string filename)
    {
        var blob = await blobs.RetrieveAsync(uuid);
        if(blob.Content == null) {
            throw new DryException("Missing content.");
        }
        if(!string.IsNullOrEmpty(filename)) {
            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{filename}\"");
        }

        await BlobSerializer.SerializeBlobAsync(Response, blob);
    }

    /// <summary>
    /// Update an existing Blob description.
    /// </summary>
    [HttpPut("api/blobs/{uuid}")]
    [HttpPut("api/blobs/{uuid}/{filename}")]
    [Authorize(SamplePolicies.SamplePolicy)]
    [SuppressMessage("ApiUsage", "DRY1107:HttpPost, HttpPut and HttpPatch methods should have Consumes attribute", Justification = "Blob controller may consume multiple mime types.")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Ignored but included for consistent consumer experience.")]
    public async Task Update(Guid uuid, string filename = "")
    {
        var headerUuidStr = Request.Headers[Blob.UuidHeaderName].FirstOrDefault() ?? "";
        if(Guid.TryParse(headerUuidStr, out var headerUuid)) {
            if(headerUuid != uuid) {
                throw new ArgumentMismatchException("UUID in header does not match UUID in URL", nameof(uuid));
            }
        }

        var exemplar = await BlobSerializer.DeserializeBlobAsync<Blob>(Request);
        await blobs.UpdateAsync(exemplar);
    }

    /// <summary>
    /// Delete an existing blob.
    /// </summary>
    [HttpDelete("api/blobs/{uuid}")]
    [Authorize(SamplePolicies.SamplePolicy)]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Ignored but included for consistent consumer experience.")]
    public async Task Delete(Guid uuid, string filename = "")
    {
        await blobs.DeleteAsync(uuid);
    }

    private readonly InMemoryBlobService blobs;

}
