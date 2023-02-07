using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ReimbursementApp.Application.Exceptions;
using ReimbursementApp.Application.Interfaces;
using ReimbursementApp.Domain.Enums;
using ReimbursementApp.Domain.Models;
using ReimbursementApp.Domain.Resources;
using ReimbursementApp.Infrastructure.Interfaces;
using UnauthorizedAccessException = ReimbursementApp.Application.Exceptions.UnauthorizedAccessException;

namespace ReimbursementApp.Application.Services;

public class EmployeeService: IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IHttpContextAccessor _httpContext;

    public EmployeeService(IEmployeeRepository employeeRepository, IHttpContextAccessor httpContext)
    {
        _employeeRepository = employeeRepository;
        _httpContext = httpContext;
    }
    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
        return await _employeeRepository.GetAll();
    }

    public async Task<Employee?> GetEmployeeById(int id)
    {
        this.VerifyAccess(id);
        var employee =  await _employeeRepository.Get(id);
        if (employee == null)
            throw new NotFoundException(Resource.EmployeeNotFound);
        return employee;
    }

    public async Task<Employee> AddNewEmployee(Employee employee)
    {
        employee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);
        return await _employeeRepository.Add(employee);
    }


    public async Task<Employee?> UpdateEmployee(Employee updatedEmployee)
    {
        this.VerifyAccess(updatedEmployee.Id);
        var employee = await GetEmployeeById(updatedEmployee.Id);
        updatedEmployee.Password = BCrypt.Net.BCrypt.Verify(updatedEmployee.Password, employee.Password)
            ? employee.Password
            : BCrypt.Net.BCrypt.HashPassword(updatedEmployee.Password);
        return await _employeeRepository.Update(updatedEmployee);
    }
    
    public async Task RemoveEmployee(int id)
    {
        var employee = await GetEmployeeById(id);
        await _employeeRepository.Delete(employee);
    }

    private void VerifyAccess(int requestedId)
    {
        if (_httpContext.HttpContext == null)
            throw new UnauthorizedAccessException(Resource.TokenInvalid);
        var actualId = int.Parse(_httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        if(actualId != requestedId && _httpContext.HttpContext.User.IsInRole(Role.Employee.ToString()))
            throw new UnauthorizedAccessException(Resource.UnAuthorized);
    }
}