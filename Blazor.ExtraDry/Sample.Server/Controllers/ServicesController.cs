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
    public class ServicesController {
       
        public ServicesController(ServicesService servicesService)
        {
            services = servicesService;
        }

        [HttpGet("api/services"), Produces("application/json")]
        [SwaggerOperation("Filtered list of all company services")]
        public async Task<FilteredCollection<Service>> List([FromQuery] FilterQuery query)
        {
            return await services.ListAsync(query);
        }

        [HttpPost("api/services"), Consumes("application/json")]
        [SwaggerOperation("Create a new company service.")]
        public async Task Create(Service value)
        {
            await services.CreateAsync(value);
        }

        [HttpGet("api/services/{uniqueId}"), Produces("application/json")]
        [SwaggerOperation("Retreive a specific company service.")]
        public async Task<Service> Retrieve(Guid uniqueId)
        {
            return await services.RetrieveAsync(uniqueId);
        }

        [HttpPut("api/services/{uniqueId}"), Consumes("application/json")]
        [SwaggerOperation("Update an existing company service.")]
        public async Task Update(Guid uniqueId, Service value)
        {
            if(uniqueId != value?.UniqueId) {
                throw new ArgumentException("ID in URI must match body.", nameof(uniqueId));
            }
            await services.UpdateAsync(value);
        }

        [HttpDelete("api/services/{uniqueId}")]
        [SwaggerOperation("Delete an existing company service.")]
        public async Task Delete(Guid uniqueId)
        {
            await services.DeleteAsync(uniqueId);
        }

        private readonly ServicesService services;
    }
}
