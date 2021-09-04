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
    public class CompanyController {
        
        public CompanyController(CompanyService companyService)
        {
            companies = companyService;
        }

        [HttpGet("api/companies"), Produces("application/json")]
        [SwaggerOperation("List all companies", "Provides a complete list of all companies, as this list is not too large, all are returned on every call.")]
        public async Task<FilteredCollection<Company>> List([FromQuery] FilterQuery query)
        {
            return await companies.List(query);
        }

        [HttpGet("api/companies/{companyId}"), Produces("application/json")]
        [SwaggerOperation("Retreive a specific company")]
        public async Task<Company> Retrieve(Guid companyId)
        {
            return await companies.Retrieve(companyId);
        }

        [HttpPost("api/companies"), Consumes("application/json")]
        [SwaggerOperation("Create a new company.", "Create a new company at the URI, the uniqueId in the URI must match the Id in the payload.")]
        public async Task Create(Company value)
        {
            await companies.Create(value);
        }

        [HttpPut("api/companies/{companyId}"), Consumes("application/json")]
        [SwaggerOperation("Update an existing company.", "Update the company at the URI, the uniqueId in the URI must match the Id in the payload.")]
        public async Task Update(Guid companyId, Company value)
        {
            if(companyId != value?.Uuid) {
                throw new ArgumentException("ID in URI must match body.", nameof(companyId));
            }
            await companies.Update(value);
        }

        [HttpDelete("api/companies/{companyId}")]
        [SwaggerOperation("Delete an existing company.")]
        public async Task Delete(Guid companyId)
        {
            await companies.Delete(companyId);
        }

        private readonly CompanyService companies;
    }
}
