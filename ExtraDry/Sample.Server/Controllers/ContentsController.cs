#nullable enable

using ExtraDry.Core;
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

        [HttpGet("api/contents/{contentId}"), Produces("application/json")]
        [SwaggerOperation("Retreive a specific page of content")]
        public async Task<Content> Retrieve(Guid contentId)
        {
            return await contents.RetrieveAsync(contentId);
        }

        [HttpPut("api/contents/{contentId}"), Consumes("application/json")]
        [SwaggerOperation("Update an existing page of content.", "Update the content at the URI, the uniqueId in the URI must match the Id in the payload.")]
        public async Task Update(Guid contentId, Content value)
        {
            if(contentId != value?.Uuid) {
                throw new ArgumentException("ID in URI must match body.", nameof(contentId));
            }
            await contents.UpdateAsync(value);
        }

        [HttpDelete("api/contents/{contentId}")]
        [SwaggerOperation("Delete an existing page of content.")]
        public async Task Delete(Guid contentId)
        {
            await contents.DeleteAsync(contentId);
        }

        private readonly ContentsService contents;
    }
}
