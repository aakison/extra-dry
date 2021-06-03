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
    public class ContentsController {
       
        public ContentsController(ContentsService contentsService)
        {
            contents = contentsService;
        }

        [HttpGet("api/contents"), Produces("application/json")]
        [SwaggerOperation("Filtered list of all contents")]
        public async Task<FilteredCollection<Content>> List([FromQuery] FilterQuery query)
        {
            return await contents.ListAsync(query);
        }

        [HttpPost("api/contents"), Consumes("application/json")]
        [SwaggerOperation("Create a new page of content.", "Create a new content entity at the URI, the uniqueId in the URI must match the Id in the payload.")]
        public async Task Create(Content value)
        {
            await contents.CreateAsync(value);
        }

        [HttpGet("api/contents/{uniqueId}"), Produces("application/json")]
        [SwaggerOperation("Retreive a specific page of content")]
        public async Task<Content> Retrieve(Guid uniqueId)
        {
            return await contents.RetrieveAsync(uniqueId);
        }

        [HttpPut("api/contents/{uniqueId}"), Consumes("application/json")]
        [SwaggerOperation("Update an existing page of content.", "Update the content at the URI, the uniqueId in the URI must match the Id in the payload.")]
        public async Task Update(Guid uniqueId, Content value)
        {
            if(uniqueId != value?.UniqueId) {
                throw new ArgumentException("ID in URI must match body.", nameof(uniqueId));
            }
            await contents.UpdateAsync(value);
        }

        [HttpDelete("api/contents/{uniqueId}")]
        [SwaggerOperation("Delete an existing page of content.")]
        public async Task Delete(Guid uniqueId)
        {
            await contents.DeleteAsync(uniqueId);
        }

        private readonly ContentsService contents;
    }
}
