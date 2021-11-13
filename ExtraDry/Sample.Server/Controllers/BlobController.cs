#nullable enable

using ExtraDry.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sample.Data.Services;
using Sample.Shared.Security;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Sample.Server.Controllers {

    /// <summary>
    /// Manages blob, such as used in Contents.
    /// </summary>
    /// <remarks>
    /// Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. 
    /// Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. 
    /// Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. 
    /// Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
    /// </remarks>
    [ApiController]
    [SkipStatusCodePages]
    [SuppressMessage("Usage", "DRY1002:ApiController shouldn't inherit from ControllerBase", Justification = "Controller makes use of ControllerBase functionality for emitting file content.")]
    public class BlobController : ControllerBase {

        /// <summary>
        /// Standard DI Constructor
        /// </summary>
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
        [AllowAnonymous]
        public async Task<PagedCollection<BlobInfo>> List([FromQuery] PageQuery query)
        {
            return await blobs.List(query);
        }

        /// <summary>
        /// Uploads a file.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
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
        /// <param name="uniqueId"></param>
        /// <returns></returns>
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
        /// "Retrieve a specific blob's information."
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
        /// <param name="uniqueId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
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
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        [HttpDelete("api/blobs/{uniqueId}")]
        [Authorize(SamplePolicies.SamplePolicy)]
        public async Task Delete(Guid uniqueId)
        {
            await blobs.DeleteAsync(uniqueId);
        }

        private readonly BlobService blobs;
    }
}
