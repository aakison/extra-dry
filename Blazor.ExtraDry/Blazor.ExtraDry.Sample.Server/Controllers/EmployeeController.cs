using Blazor.ExtraDry.Sample.Data.Services;
using Blazor.ExtraDry.Sample.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Blazor.ExtraDry.Sample.Server.Controllers {

    public class EmployeeController : ApiController {
        
        public EmployeeController()
        {
            // Note: In non-sample/production app, replace this with DI.
            employees = new EmployeeService();
        }

        [HttpGet, Route("employees")]
        public async Task<IEnumerable<Employee>> List()
        {
            return await employees.List();
        }

        [HttpGet, Route("employees/{uniqueId}")]
        public async Task<Employee> Retrieve(Guid uniqueId)
        {
            return await employees.Retrieve(uniqueId);
        }

        [HttpPost, Route("employees/{uniqueId}")]
        public async Task Create(Guid uniqueId, [FromBody] Employee value)
        {
            if(uniqueId != value?.UniqueId) {
                throw new ArgumentException("ID in URI must match body.", nameof(uniqueId));
            }
            await employees.Create(value);
        }

        [HttpPut, Route("employees/{uniqueId}")]
        public async Task Update(Guid uniqueId, [FromBody] Employee value)
        {
            if(uniqueId != value?.UniqueId) {
                throw new ArgumentException("ID in URI must match body.", nameof(uniqueId));
            }
            await employees.Update(value);
        }

        [HttpDelete, Route("employees/{uniqueId}")]
        public async Task Delete(Guid uniqueId)
        {
            await employees.Delete(uniqueId);
        }

        private readonly EmployeeService employees;
    }
}
