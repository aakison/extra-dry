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
    public class SectorController {
       
        public SectorController(SectorService sectorService)
        {
            sectors = sectorService;
        }

        [HttpGet("api/sectors"), Produces("application/json")]
        [SwaggerOperation("Filtered list of all company services")]
        public async Task<FilteredCollection<Sector>> List([FromQuery] FilterQuery query)
        {
            return await sectors.ListAsync(query);
        }

        [HttpPost("api/sectors"), Consumes("application/json")]
        [SwaggerOperation("Create a new company service.")]
        public async Task Create(Sector value)
        {
            await sectors.CreateAsync(value);
        }

        [HttpGet("api/sectors/{uniqueId}"), Produces("application/json")]
        [SwaggerOperation("Retreive a specific company service.")]
        public async Task<Sector> Retrieve(Guid uniqueId)
        {
            return await sectors.RetrieveAsync(uniqueId);
        }

        [HttpPut("api/sectors/{uniqueId}"), Consumes("application/json")]
        [SwaggerOperation("Update an existing company service.")]
        public async Task Update(Guid uniqueId, Sector value)
        {
            if(uniqueId != value?.UniqueId) {
                throw new ArgumentException("ID in URI must match body.", nameof(uniqueId));
            }
            await sectors.UpdateAsync(value);
        }

        [HttpDelete("api/sectors/{uniqueId}")]
        [SwaggerOperation("Delete an existing company service.")]
        public async Task Delete(Guid uniqueId)
        {
            await sectors.DeleteAsync(uniqueId);
        }

        private readonly SectorService sectors;
    }
}
