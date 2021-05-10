#nullable enable

using Sample.Data.Services;
using Sample.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using Blazor.ExtraDry;

namespace Sample.Server.Controllers {

    public class EmployeeController : Controller {
       
        public EmployeeController(EmployeeService employeeService)
        {
            employees = employeeService;
        }

        [HttpGet("api/employees")]
        [SwaggerOperation("Paged list of all employees", "As a large number of employees are in the system, this allows for a set of query parameters to determine which subset of the total collection to return.  If too many results are present, the output collection will return a page of them along with a continuation token to use to consistently retrieve additional results.")]
        public async Task<PartialCollection<Employee>> List([FromQuery] PartialQuery query)
        {
            return await employees.List(query);
        }

        [HttpPost("api/employees/{uniqueId}")]
        [SwaggerOperation("Create a new employee.", "Create a new employee at the URI, the uniqueId in the URI must match the Id in the payload.")]
        public async Task Create(Guid uniqueId, [FromBody] Employee value)
        {
            if(uniqueId != value?.UniqueId) {
                throw new ArgumentException("ID in URI must match body.", nameof(uniqueId));
            }
            await employees.Create(value);
        }

        [HttpGet("api/employees/{uniqueId}")]
        [SwaggerOperation("Retreive a specific employee")]
        public async Task<Employee> Retrieve(Guid uniqueId)
        {
            return await employees.Retrieve(uniqueId);
        }

        [HttpPut("api/employees/{uniqueId}")]
        [SwaggerOperation("Update an existing employee.", "Update the employee at the URI, the uniqueId in the URI must match the Id in the payload.")]
        public async Task Update(Guid uniqueId, [FromBody] Employee value)
        {
            if(uniqueId != value?.UniqueId) {
                throw new ArgumentException("ID in URI must match body.", nameof(uniqueId));
            }
            await employees.Update(value);
        }

        [HttpDelete("api/employees/{uniqueId}")]
        [SwaggerOperation("Delete an existing employee.")]
        public async Task Delete(Guid uniqueId)
        {
            await employees.Delete(uniqueId);
        }

        private readonly EmployeeService employees;
    }
}
