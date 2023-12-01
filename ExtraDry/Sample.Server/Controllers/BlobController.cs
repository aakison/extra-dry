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
    /// Uploads a blob, both BlobInfo and content in a single call.  As the content is delivered as 
    /// the body of the Request, the BlobInfo must be delivered in the header.
    /// </summary>
    [HttpPost("/api/blobs/{uuid}")]
    [Produces("application/json")]
    [AllowAnonymous]
    [SuppressMessage("ApiUsage", "DRY1107:HttpPost, HttpPut and HttpPatch methods should have Consumes attribute", Justification = "Adjusts to multiple mime types and can't explicitly state consumption of */*")]
    public async Task<ResourceReference<BlobInfo>> CreateBlobAsync(Guid uuid)
    {
        var edUuidStr = Request.Headers[BlobInfo.UuidHeaderName].FirstOrDefault();
        var edUuid = Guid.Parse(edUuidStr ?? "");
        var mimeType = Request.Headers[BlobInfo.MimeTypeHeaderName].FirstOrDefault() ?? "";
        var title = Request.Headers[BlobInfo.TitleHeaderName].FirstOrDefault() ?? "";
        var scopeStr = Request.Headers[BlobInfo.ScopeHeaderName].FirstOrDefault();
        var scope = Enum.Parse<BlobScope>(scopeStr ?? "");
        var shaHash = Request.Headers[BlobInfo.ShaHashHeaderName].FirstOrDefault();
        var slug = Request.Headers[BlobInfo.SlugHeaderName].FirstOrDefault() ?? "";
        
        if(edUuid != uuid) {
            throw new DryException("UUID in header does not match UUID in URL");
        }

        var memoryStream = new MemoryStream();
        await Request.Body.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();
        var item = new BlobInfo {
            MimeType = mimeType,
            Title = title,
            Scope = scope,
            Slug = slug,
            ShaHash = shaHash ?? "",
            Size = bytes.Length,
            Uuid = uuid,
        };
        var blob = await blobs.UploadAsync(item, bytes);
        return new ResourceReference<BlobInfo>(blob);
    }

    ///// <summary>
    ///// Uploads a file.
    ///// </summary>
    //[HttpPost("api/blobs/{scope}")]
    //[Consumes("multipart/form-data"), Produces("application/json")]
    //[Authorize(SamplePolicies.SamplePolicy)]
    //public async Task<ResourceReference<BlobInfo>> CreateBlobAsync(BlobScope scope, [FromQuery] string filename, [FromQuery] string mimeType)
    //{
    //    var memoryStream = new MemoryStream();
    //    await Request.Body.CopyToAsync(memoryStream);
    //    var bytes = memoryStream.ToArray();
    //    var item = new BlobInfo {
    //        MimeType = mimeType,
    //        Title = filename,
    //        Scope = scope,
    //        Size = bytes.Length,
    //    };
    //    var blob = await blobs.UploadAsync(item, bytes);
    //    return new ResourceReference<BlobInfo>(blob);
    //}

    /// <summary>
    /// Download the content of specified blob.
    /// </summary>
    [HttpGet("api/blobs/{uuid}/content")]
    [Produces("application/octet-stream")]
    [AllowAnonymous]
    public async Task<FileContentResult> RetrieveAsync(Guid uuid)
    {
        var blob = await blobs.RetrieveAsync(uuid);
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
    [HttpDelete("api/blobs/{uniqueId}")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task Delete(Guid uniqueId)
    {
        await blobs.DeleteAsync(uniqueId);
    }

    private readonly BlobService blobs;

}
