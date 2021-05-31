#nullable enable

using Blazor.ExtraDry;
using Microsoft.AspNetCore.Mvc;
using Sample.Data.Services;
using Sample.Shared;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace Sample.Server.Controllers {

    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ContentsController {
       
        public ContentsController(ContentsService contentsService)
        {
            contents = contentsService;
        }

        [HttpGet("api/contents")]
        [SwaggerOperation("Filtered list of all contents")]
        public async Task<FilteredCollection<Content>> List([FromQuery] FilterQuery query)
        {
            return await contents.List(query);
        }

        [HttpPost("api/contents")]
        [SwaggerOperation("Create a new page of content.", "Create a new content entity at the URI, the uniqueId in the URI must match the Id in the payload.")]
        public async Task Create(Content value)
        {
            await contents.Create(value);
        }

        [HttpGet("api/contents/{uniqueId}")]
        [SwaggerOperation("Retreive a specific page of content")]
        public async Task<Content> Retrieve(Guid uniqueId)
        {
            return await contents.Retrieve(uniqueId);
        }

        [HttpPut("api/contents/{uniqueId}")]
        [SwaggerOperation("Update an existing page of content.", "Update the content at the URI, the uniqueId in the URI must match the Id in the payload.")]
        public async Task Update(Guid uniqueId, Content value)
        {
            if(uniqueId != value?.UniqueId) {
                throw new ArgumentException("ID in URI must match body.", nameof(uniqueId));
            }
            await contents.Update(value);
        }

        [HttpDelete("api/contents/{uniqueId}")]
        [SwaggerOperation("Delete an existing page of content.")]
        public async Task Delete(Guid uniqueId)
        {
            await contents.Delete(uniqueId);
        }

        private readonly ContentsService contents;
    }
}
