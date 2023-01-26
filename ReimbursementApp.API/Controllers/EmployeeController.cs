using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using ReimbursementApp.API.Configurations;
using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Application.Interfaces;
using ReimbursementApp.Domain.Constants;
using ReimbursementApp.Domain.Enums;
using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.API.Controllers;

[ApiController]
[Authorize]
[Route("api/employee")]
public class EmployeeController:ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IMapper mapper,IEmployeeService employeeService)
    {
        _mapper = mapper;
        _employeeService = employeeService;
    }
    
    [HttpGet]
    [Route("all")]
    [AuthorizeRoles( Role.Admin,Role.Manager )]
    public async Task<ActionResult> ListAllEmployees()
    {
        var employees = await _employeeService.GetAllEmployees();
        var result = employees.Select(employee => _mapper.Map<EmployeeResponseDto>(employee));
        
        var response = new { message=EmployeeConstants.EmployeesFetched, result =result};
        return new OkObjectResult(response);
    }
    
    
    [HttpGet]
    public async Task<ActionResult> GetEmployee(int id)
    {
        var employee = await _employeeService.GetEmployeeById(id);
        var result = _mapper.Map<EmployeeResponseDto>(employee);
        
        return new OkObjectResult(new { message = EmployeeConstants.EmployeeFetched, result = result });
    }
    
    [HttpPost]
    [AuthorizeRoles(Role.Admin)]
    public async Task<ActionResult> AddEmployee(EmployeeDto employee)
    {
        var addedEmployee = await _employeeService.AddNewEmployee(_mapper.Map<Employee>(employee));

    
        var response = new { message=EmployeeConstants.EmployeeAdded, result = _mapper.Map<EmployeeResponseDto>(addedEmployee)};
        return new OkObjectResult(response);
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateEmployee(EmployeeUpdateRequestDto employee)
    {
        var result = await _employeeService.UpdateEmployee(_mapper.Map<Employee>(employee));
        var response = new
            { message = EmployeeConstants.EmployeeUpdated, result = _mapper.Map<EmployeeResponseDto>(result) };
        return new OkObjectResult(response);
    }
    
    [HttpDelete]
    [AuthorizeRoles(Role.Admin)]
    public async Task<ActionResult> TerminateEmployee(int id)
    {
         await _employeeService.RemoveEmployee(id);
         return Ok(EmployeeConstants.EmployeeDeleted);
    }
}