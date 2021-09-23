#nullable enable

using ExtraDry.Core;
using Microsoft.AspNetCore.Mvc;
using Sample.Data.Services;
using Sample.Shared;
using System;
using System.Threading.Tasks;

namespace Sample.Server.Controllers {

    [ApiController]
    public class CompanyController {
        
        public CompanyController(CompanyService companyService)
        {
            companies = companyService;
        }

        /// <summary>
        /// Filtered list of all companies
        /// </summary>
        /// <remarks>
        /// Provides a complete list of all companies, as this list is not too large, all are returned on every call.
        /// </remarks>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("api/companies"), Produces("application/json")]
        public async Task<FilteredCollection<Company>> List([FromQuery] FilterQuery query)
        {
            return await companies.List(query);
        }

        /// <summary>
        /// Retrieve a specific company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet("api/companies/{companyId}"), Produces("application/json")]
        public async Task<Company> Retrieve(Guid companyId)
        {
            return await companies.Retrieve(companyId);
        }

        /// <summary>
        /// Create a new company
        /// </summary>
        /// <remarks>
        /// Create a new company at the URI, the uniqueId in the URI must match the Id in the payload.
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("api/companies"), Consumes("application/json")]
        public async Task Create(Company value)
        {
            await companies.Create(value);
        }

        /// <summary>
        /// Update an existing company
        /// </summary>
        /// <remarks>
        /// Update the company at the URI, the uniqueId in the URI must match the Id in the payload.
        /// </remarks>
        /// <param name="companyId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("api/companies/{companyId}"), Consumes("application/json")]
        public async Task Update(Guid companyId, Company value)
        {
            if(companyId != value?.Uuid) {
                throw new ArgumentException("ID in URI must match body.", nameof(companyId));
            }
            await companies.Update(value);
        }

        /// <summary>
        /// Delete an existing company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpDelete("api/companies/{companyId}")]
        public async Task Delete(Guid companyId)
        {
            await companies.Delete(companyId);
        }

        private readonly CompanyService companies;
    }
}
