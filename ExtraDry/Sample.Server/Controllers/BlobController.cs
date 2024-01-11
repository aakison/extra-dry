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
    public BlobController(BlobService blobService)
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

    ///// <summary>
    ///// Update an existing blob description.
    ///// </summary>
    ///// <remarks>
    ///// Updates information about the indicated blob, to change the blob content use the `/upload` endpoint.
    ///// </remarks>
    //[HttpPut("api/blobs/{uniqueId}")]
    //[Consumes("multipart/form-data")]
    //[Authorize(SamplePolicies.SamplePolicy)]
    //public async Task Update(Guid uniqueId, [FromBody] BlobInfo value)
    //{
    //    if(uniqueId != value?.Uuid) {
    //        throw new ArgumentException("ID in URI must match body.", nameof(uniqueId));
    //    }
    //    await blobs.UpdateAsync(value);
    //}

    /// <summary>
    /// Delete an existing blob.
    /// </summary>
    //[HttpDelete("api/blobs/{uniqueId}")]
    //[Authorize(SamplePolicies.SamplePolicy)]
    //public async Task Delete(Guid uniqueId)
    //{
    //    await blobs.DeleteAsync(uniqueId);
    //}

    private readonly BlobService blobs;

}
