#nullable enable

using ExtraDry.Core;
using Microsoft.AspNetCore.Mvc;
using Sample.Data.Services;
using Sample.Shared;
using System;
using System.Threading.Tasks;

namespace Sample.Server.Controllers {

    [ApiController]
    public class ContentsController {
       
        public ContentsController(ContentsService contentsService)
        {
            contents = contentsService;
        }

        /// <summary>
        /// Filtered list of all contents
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("api/contents"), Produces("application/json")]
        public async Task<FilteredCollection<Content>> List([FromQuery] FilterQuery query)
        {
            return await contents.ListAsync(query);
        }

        /// <summary>
        /// Create a new page of content
        /// </summary>
        /// <remarks>
        /// Create a new content entity at the URI, the uniqueId in the URI must match the Id in the payload.
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("api/contents"), Consumes("application/json")]
        public async Task Create(Content value)
        {
            await contents.CreateAsync(value);
        }

        /// <summary>
        /// Retreive a specific page of content
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        [HttpGet("api/contents/{contentId}"), Produces("application/json")]
        public async Task<Content> Retrieve(Guid contentId)
        {
            return await contents.RetrieveAsync(contentId);
        }

        /// <summary>
        /// Update an existing page of content
        /// </summary>
        /// <remarks>
        /// Update the content at the URI, the uniqueId in the URI must match the Id in the payload.
        /// </remarks>
        /// <param name="contentId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("api/contents/{contentId}"), Consumes("application/json")]
        public async Task Update(Guid contentId, Content value)
        {
            if(contentId != value?.Uuid) {
                throw new ArgumentException("ID in URI must match body.", nameof(contentId));
            }
            await contents.UpdateAsync(value);
        }

        /// <summary>
        /// Delete an existing page of content
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        [HttpDelete("api/contents/{contentId}")]
        public async Task Delete(Guid contentId)
        {
            await contents.DeleteAsync(contentId);
        }

        private readonly ContentsService contents;
    }
}
