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
    public class EmployeeController {
       
        public EmployeeController(EmployeeService employeeService)
        {
            employees = employeeService;
        }

        [HttpGet("api/employees"), Produces("application/json")]
        [SwaggerOperation("Paged list of all employees", "As a large number of employees are in the system, this allows for a set of query parameters to determine which subset of the total collection to return.  If too many results are present, the output collection will return a page of them along with a continuation token to use to consistently retrieve additional results.")]
        public async Task<PagedCollection<Employee>> List([FromQuery] PageQuery query)
        {
            return await employees.List(query);
        }

        [HttpPost("api/employees"), Consumes("application/json")]
        [SwaggerOperation("Create a new employee.", "Create a new employee at the URI, the uniqueId in the URI must match the Id in the payload.")]
        public async Task Create(Employee value)
        {
            await employees.Create(value);
        }

        [HttpGet("api/employees/{employeeId}"), Produces("application/json")]
        [SwaggerOperation("Retreive a specific employee")]
        public async Task<Employee> Retrieve(Guid employeeId)
        {
            return await employees.Retrieve(employeeId);
        }

        [HttpPut("api/employees/{employeeId}"), Consumes("application/json")]
        [SwaggerOperation("Update an existing employee.", "Update the employee at the URI, the uniqueId in the URI must match the Id in the payload.")]
        public async Task Update(Guid employeeId, Employee value)
        {
            if(employeeId != value?.Uuid) {
                throw new ArgumentException("ID in URI must match body.", nameof(employeeId));
            }
            await employees.Update(value);
        }

        [HttpDelete("api/employees/{employeeId}")]
        [SwaggerOperation("Delete an existing employee.")]
        public async Task Delete(Guid employeeId)
        {
            await employees.Delete(employeeId);
        } 

        private readonly EmployeeService employees;
    }
}
