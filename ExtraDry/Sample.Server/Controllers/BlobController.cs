#nullable enable

using ExtraDry.Core;
using Microsoft.AspNetCore.Mvc;
using Sample.Data.Services;
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

        /// <summary>
        /// Paginated list of all blobs
        /// </summary>
        /// <remarks>
        /// Provides a paged list of all blobs.
        /// </remarks>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("api/blobs")]
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

        /// <summary>
        /// Download the content of specified blob.
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        [HttpGet("api/blobs/{uniqueId}/content")]
        [Produces("application/octet-stream")]
        public async Task<FileContentResult> DownloadAsync(Guid uniqueId)
        {
            var blob = await blobs.RetrieveAsync(uniqueId);
            var content = await blobs.DownloadAsync(blob);
            return File(content, blob.MimeType);
        }

        /// <summary>
        /// "Retrieve a specific blob's information."
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpGet("api/blobs/{uuid}")]
        [Produces("application/json")]
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
        /// <param name="uniqueId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("api/blobs/{uniqueId}")]
        [Consumes("application/octet-stream")]
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
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        [HttpDelete("api/blobs/{uniqueId}")]
        public async Task Delete(Guid uniqueId)
        {
            await blobs.DeleteAsync(uniqueId);
        }

        private readonly BlobService blobs;
    }
}
