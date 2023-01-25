using System.Text.Json.Nodes;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReimbursementApp.API.Utils;
using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Application.Interfaces;
using ReimbursementApp.Domain.Constants;
using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.API.Controllers;

[ApiController]
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
    public ActionResult<List<EmployeeResponseDto>> ListAllEmployees()
    {
        var employees = _employeeService.GetAllEmployees();
        var result = employees.Select(employee => _mapper.Map<EmployeeResponseDto>(employee));
        
        var response = ApiResponseHandler.OkResponse(EmployeeConstants.EmployeesFetched, result);
        return Ok(response);
    }
    
    
    [HttpGet]
    public ActionResult<EmployeeResponseDto> GetEmployee(int id)
    {
        var employee = _employeeService.GetEmployeeById(id);
        var result = _mapper.Map<EmployeeResponseDto>(employee);
        var response = ApiResponseHandler.OkResponse(EmployeeConstants.EmployeeFetched, result);
        return Ok(response);
    }
    
    [HttpPost]
    public ActionResult AddEmployee(EmployeeDto employee)
    {
        var addedEmployee = _employeeService.AddNewEmployee(_mapper.Map<Employee>(employee));

        var response = ApiResponseHandler.OkResponse(EmployeeConstants.EmployeeAdded,
            _mapper.Map<EmployeeResponseDto>(addedEmployee));
        return Ok(response);
    }
    
    
    [HttpDelete]
    public ActionResult TerminateEmployee(int id)
    {
        _employeeService.RemoveEmployee(id);
        var response = ApiResponseHandler.OkResponse(EmployeeConstants.EmployeeDeleted);
        return Ok(response);
    }

    [HttpPut]
    public ActionResult UpdateEmployee(EmployeeUpdateRequestDto employee)
    {
        _employeeService.UpdateEmployee(_mapper.Map<Employee>(employee));
        var response = ApiResponseHandler.OkResponse(EmployeeConstants.EmployeeUpdated);
        return Ok(response);
    }
    

}