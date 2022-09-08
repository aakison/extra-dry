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
public class BlobController : ControllerBase {

    /// <summary>
    /// Standard DI Constructor
    /// </summary>
    public BlobController(BlobService blobService)
    {
        blobs = blobService;
    }

    /// <summary>
    /// Provides a paged list of all blobs.
    /// </summary>
    [HttpGet("api/blobs")]
    [Produces("application/json")]
    [AllowAnonymous]
    public async Task<PagedCollection<BlobInfo>> List([FromQuery] PageQuery query)
    {
        return await blobs.List(query);
    }

    /// <summary>
    /// Uploads a file.
    /// </summary>
    [HttpPost("api/blobs/{scope}/{filename}")]
    [Consumes("multipart/form-data"), Produces("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task<BlobInfo> CreateBlobAsync(BlobScope scope, string filename)
    {
        var memoryStream = new MemoryStream();
        await Request.Body.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();
        var item = new BlobInfo {
            Filename = filename,
            Scope = scope,
            Size = bytes.Length,
        };
        return await blobs.UploadAsync(item, bytes);
    }

    /// <summary>
    /// Download the content of specified blob.
    /// </summary>
    [HttpGet("api/blobs/{uniqueId}/content")]
    [Produces("application/octet-stream")]
    [AllowAnonymous]
    public async Task<FileContentResult> RetrieveAsync(Guid uniqueId)
    {
        var blob = await blobs.RetrieveAsync(uniqueId);
        var content = await blobs.DownloadAsync(blob);
        return File(content, blob.MimeType);
    }

    /// <summary>
    /// Retrieve a specific blob's information.
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    [HttpGet("api/blobs/{uuid}")]
    [Produces("application/json")]
    [AllowAnonymous]
    public async Task<BlobInfo> Retrieve(Guid uuid)
    {
        return await blobs.RetrieveAsync(uuid);
    }

    /// <summary>
    /// Update an existing blob description.
    /// </summary>
    /// <remarks>
    /// Updates information about the indicated blob, to change the blob content use the `/upload` endpoint.
    /// </remarks>
    [HttpPut("api/blobs/{uniqueId}")]
    [Consumes("multipart/form-data")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task Update(Guid uniqueId, [FromBody] BlobInfo value)
    {
        if(uniqueId != value?.UniqueId) {
            throw new ArgumentException("ID in URI must match body.", nameof(uniqueId));
        }
        await blobs.UpdateAsync(value);
    }

    /// <summary>
    /// Delete an existing blob.
    /// </summary>
    [HttpDelete("api/blobs/{uniqueId}")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task Delete(Guid uniqueId)
    {
        await blobs.DeleteAsync(uniqueId);
    }

    private readonly BlobService blobs;
}
