#nullable enable

using Blazor.ExtraDry;
using Microsoft.AspNetCore.Mvc;
using Sample.Data.Services;
using Sample.Shared;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Server.Controllers {

    [ApiController]
    public class BlobController : ControllerBase {
        public BlobController(BlobService companyService)
        {
            blobs = companyService;
        }

        [HttpGet("api/blobs")]
        [SwaggerOperation("List all blobs", "Provides a paged list of all blobs.")]
        public async Task<PagedCollection<BlobInfo>> List([FromQuery] PageQuery query)
        {
            return await blobs.List(query);
        }

        [HttpPost("api/blobs/{scope}/{name}")]
        public async Task Create(string scope, string name)
        {
            using var bodyStream = Request.BodyReader.AsStream();
            var item = new BlobInfo {
                Name = name,
                Scope = scope,
            };
            await blobs.Create(item, bodyStream, (int)(Request.ContentLength ?? 0));
        }

        [HttpGet("api/blobs/{uniqueId}")]
        [SwaggerOperation("Retrieve a specific blob's information.")]
        public async Task<BlobInfo> Retrieve(Guid uniqueId)
        {
            return await blobs.Retrieve(uniqueId);
        }

        [HttpGet("api/blobs/{uniqueId}/content")]
        [SwaggerOperation("Download the content of specified blob.")]
        public async Task<ActionResult> Download(Guid uniqueId)
        {
            var blob = await blobs.Retrieve(uniqueId);
            return Content(blob.ToString());
        }

        [HttpPut("api/blobs/{uniqueId}")]
        [SwaggerOperation("Update an existing blob description.", "Updates information about the indicated blob, to change the blob content use the `/upload` endpoint.")]
        public async Task Update(Guid uniqueId, [FromBody] BlobInfo value)
        {
            if(uniqueId != value?.UniqueId) {
                throw new ArgumentException("ID in URI must match body.", nameof(uniqueId));
            }
            await blobs.Update(value);
        }

        [HttpPut("api/blobs/{uniqueId}/content")]
        [SwaggerOperation("Uploads the content of the specified blob.")]
        public async Task Upload(Guid uniqueId)
        {
            var blob = await blobs.Retrieve(uniqueId);
            //return Content(blob.ToString());
        }

        [HttpDelete("api/blobs/{uniqueId}")]
        [SwaggerOperation("Delete an existing blob.")]
        public async Task Delete(Guid uniqueId)
        {
            await blobs.Delete(uniqueId);
        }

        private readonly BlobService blobs;
    }
}
