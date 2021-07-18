#nullable enable

using Blazor.ExtraDry;
using Microsoft.AspNetCore.Mvc;
using Sample.Data.Services;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sample.Server.Controllers {

    [ApiController]
    public class BlobController : ControllerBase {

        public BlobController(BlobService blobService)
        {
            blobs = blobService;
        }

        [HttpGet("api/blobs")]
        [SwaggerOperation("List all blobs", "Provides a paged list of all blobs.")]
        [Produces("application/json")]
        public async Task<PagedCollection<BlobInfo>> List([FromQuery] PageQuery query)
        {
            return await blobs.List(query);
        }

        [HttpPost("api/blobs/{scope}/{filename}")]
        [Consumes("application/octet-stream"), Produces("application/json")]
        public async Task<BlobInfo> UploadAsync(BlobScope scope, string filename)
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

        [HttpGet("api/blobs/{uniqueId}/content")]
        [SwaggerOperation("Download the content of specified blob.")]
        [Produces("application/octet-stream")]
        public async Task<FileContentResult> DownloadAsync(Guid uniqueId)
        {
            var blob = await blobs.RetrieveAsync(uniqueId);
            var content = await blobs.DownloadAsync(blob);
            return File(content, blob.MimeType);
        }

        [HttpGet("api/blobs/{uniqueId}")]
        [SwaggerOperation("Retrieve a specific blob's information.")]
        [Produces("application/json")]
        public async Task<BlobInfo> Retrieve(Guid uniqueId)
        {
            return await blobs.RetrieveAsync(uniqueId);
        }

        [HttpPut("api/blobs/{uniqueId}")]
        [SwaggerOperation("Update an existing blob description.", "Updates information about the indicated blob, to change the blob content use the `/upload` endpoint.")]
        [Consumes("application/octet-stream")]
        public async Task Update(Guid uniqueId, [FromBody] BlobInfo value)
        {
            if(uniqueId != value?.UniqueId) {
                throw new ArgumentException("ID in URI must match body.", nameof(uniqueId));
            }
            await blobs.UpdateAsync(value);
        }

        [HttpDelete("api/blobs/{uniqueId}")]
        [SwaggerOperation("Delete an existing blob.")]
        public async Task Delete(Guid uniqueId)
        {
            await blobs.DeleteAsync(uniqueId);
        }

        private readonly BlobService blobs;
    }
}
