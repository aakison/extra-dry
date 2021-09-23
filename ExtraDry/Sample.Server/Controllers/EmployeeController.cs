#nullable enable

using ExtraDry.Core;
using Microsoft.AspNetCore.Mvc;
using Sample.Data.Services;
using Sample.Shared;
using System;
using System.Threading.Tasks;

namespace Sample.Server.Controllers {

    [ApiController]
    public class EmployeeController {
       
        public EmployeeController(EmployeeService employeeService)
        {
            employees = employeeService;
        }

        /// <summary>
        /// Paginated list of all employees
        /// </summary>
        /// <remarks>
        /// As a large number of employees are in the system, this allows for a set of query parameters to determine which 
        /// subset of the total collection to return.  If too many results are present, the output collection will return 
        /// a page of them along with a continuation token to use to consistently retrieve additional results.
        /// </remarks>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("api/employees"), Produces("application/json")]
        public async Task<PagedCollection<Employee>> List([FromQuery] PageQuery query)
        {
            return await employees.List(query);
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        /// <remarks>
        /// Create a new employee at the URI, the uniqueId in the URI must match the Id in the payload.
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("api/employees"), Consumes("application/json")]
        public async Task Create(Employee value)
        {
            await employees.Create(value);
        }

        /// <summary>
        /// Retreive a specific employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("api/employees/{employeeId}"), Produces("application/json")]
        public async Task<Employee> Retrieve(Guid employeeId)
        {
            return await employees.Retrieve(employeeId);
        }

        /// <summary>
        /// Update an existing employee
        /// </summary>
        /// <remarks>
        /// Update the employee at the URI, the uniqueId in the URI must match the Id in the payload.
        /// </remarks>
        /// <param name="employeeId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("api/employees/{employeeId}"), Consumes("application/json")]
        public async Task Update(Guid employeeId, Employee value)
        {
            if(employeeId != value?.Uuid) {
                throw new ArgumentException("ID in URI must match body.", nameof(employeeId));
            }
            await employees.Update(value);
        }

        /// <summary>
        /// Delete an existing employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpDelete("api/employees/{employeeId}")]
        public async Task Delete(Guid employeeId)
        {
            await employees.Delete(employeeId);
        } 

        private readonly EmployeeService employees;
    }
}
