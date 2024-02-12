using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Spa.Backend.Controllers;

/// <summary>
/// Manages colleciton of employees.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiGroupNames.SampleApi)]
[ApiExceptionStatusCodes]
public class EmployeeController {
       
    /// <summary>
    /// Standard DI Constructor
    /// </summary>
    /// <param name="employeeService"></param>
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
    [AllowAnonymous]
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
    [HttpPost("api/employees"), Consumes("application/json"), Produces("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task<ResourceReference<Employee>> CreateAsync(Employee value)
    {
        var employee = await employees.CreateAsync(value);
        return new ResourceReference<Employee>(employee);
    }

    /// <summary>
    /// Retreive a specific employee
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    [HttpGet("api/employees/{employeeId}"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<Employee> Retrieve(Guid employeeId)
    {
        return await employees.RetrieveAsync(employeeId);
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
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task Update(Guid employeeId, Employee value)
    {
        if(employeeId != value?.Uuid) {
            throw new ArgumentMismatchException("ID in URI must match body.", nameof(employeeId));
        }
        await employees.Update(value);
    }

    /// <summary>
    /// Delete an existing employee
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    [HttpDelete("api/employees/{employeeId}")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task Delete(Guid employeeId)
    {
        await employees.Delete(employeeId);
    } 

    private readonly EmployeeService employees;
}
