﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample.Spa.Backend.Controllers;

/// <summary>
/// Manages collection of employees.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiGroupNames.SampleApi)]
[ApiExceptionStatusCodes]
public class EmployeeController(
    EmployeeService employeeService)
{
    /// <summary>
    /// Paginated list of all employees
    /// </summary>
    /// <remarks>
    /// As a large number of employees are in the system, this allows for a set of query parameters
    /// to determine which subset of the total collection to return. If too many results are
    /// present, the output collection will return a page of them along with a continuation token
    /// to use to consistently retrieve additional results.
    /// </remarks>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("api/employees"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<PagedCollection<Employee>> List([FromQuery] PageQuery query)
    {
        return await employeeService.List(query);
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
        var employee = await employeeService.CreateAsync(value);
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
        return await employeeService.RetrieveAsync(employeeId);
    }

    /// <summary>
    /// Update an existing employee
    /// </summary>
    /// <remarks>
    /// Update the employee at the URI, the uniqueId in the URI must match the Id in the payload.
    /// </remarks>
    /// <param name="uuid"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPut("api/employees/{uuid}"), Consumes("application/json")]
    [Authorize(SamplePolicies.SamplePolicy)]
    public async Task Update(Guid uuid, Employee value)
    {
        ArgumentMismatchException.ThrowIfMismatch(uuid, value.Uuid, nameof(uuid));
        await employeeService.Update(value);
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
        await employeeService.Delete(employeeId);
    }
}
