using Sample.Data.Services;
using Sample.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace Sample.Server.Controllers {

    public class CompanyController : Controller {
        
        public CompanyController(CompanyService companyService)
        {
            companies = companyService;
        }

        [HttpGet("api/companies")]
        [SwaggerOperation("List all companies")]
        public async Task<IEnumerable<Company>> List()
        {
            return await companies.List();
        }

        [HttpGet("api/companies/{uniqueId}")]
        [SwaggerOperation("Retreive a specific company")]
        public async Task<Company> Retrieve(Guid uniqueId)
        {
            return await companies.Retrieve(uniqueId);
        }

        [HttpPost("api/companies/{uniqueId}")]
        [SwaggerOperation("Create a new company.", "Create a new company at the URI, the uniqueId in the URI must match the Id in the payload.")]
        public async Task Create(Guid uniqueId, [FromBody] Company value)
        {
            if(uniqueId != value?.UniqueId) {
                throw new ArgumentException("ID in URI must match body.", nameof(uniqueId));
            }
            await companies.Create(value);
        }

        [HttpPut("api/companies/{uniqueId}")]
        [SwaggerOperation("Update an existing company.", "Update the company at the URI, the uniqueId in the URI must match the Id in the payload.")]
        public async Task Update(Guid uniqueId, [FromBody] Company value)
        {
            if(uniqueId != value?.UniqueId) {
                throw new ArgumentException("ID in URI must match body.", nameof(uniqueId));
            }
            await companies.Update(value);
        }

        [HttpDelete("api/companies/{uniqueId}")]
        [SwaggerOperation("Delete an existing company.")]
        public async Task Delete(Guid uniqueId)
        {
            await companies.Delete(uniqueId);
        }

        private readonly CompanyService companies;
    }
}
