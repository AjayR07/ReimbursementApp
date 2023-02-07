using System.Security.Claims;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReimbursementApp.API.Controllers.v2;
using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Application.Mappers;
using ReimbursementApp.Application.Services;
using ReimbursementApp.Domain.Enums;
using ReimbursementApp.Domain.Models;
using ReimbursementApp.Domain.Resources;
using ReimbursementApp.Infrastructure.Interfaces;
using UnauthorizedAccessException = ReimbursementApp.Application.Exceptions.UnauthorizedAccessException;

namespace ReimbursementApp.Test;

public class EmployeeControllerTest
{
    private readonly Mock<IEmployeeRepository> employeeRepository;
    private readonly Mock<IHttpContextAccessor> httpContext;
    private readonly EmployeeController employeeController;


    public EmployeeControllerTest()
    {
        employeeRepository = new Mock<IEmployeeRepository>();
        httpContext =new Mock<IHttpContextAccessor>();
        var employeeService = new Mock<EmployeeService>(employeeRepository.Object, httpContext.Object);
 
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new EmployeeProfile());
        });
        var mapper = mockMapper.CreateMapper();
        employeeController = new EmployeeController(mapper,employeeService.Object);
    }
    
    [Fact]
    public async void ListAllEmployeesCheck()
    {
        // Arrange
        var employees = this.GetEmployeeList();
        employeeRepository.Setup(x => x.GetAll()).ReturnsAsync(employees);
        var expectedResult = this.GetEmployeeDtoList();
        
        // Act
        var actionResult = await employeeController.ListAllEmployees();
        
        // Assert
        Assert.IsType<OkObjectResult>(actionResult);
        Assert.NotNull(actionResult);
        
        var okObjectResult =actionResult as OkObjectResult;
        var actualResult =okObjectResult!.Value as ResponseDto;
        Assert.NotNull(actualResult);
        Assert.Equal(Resource.EmployeesFetched,actualResult!.message);
        Assert.Equal(JsonSerializer.Serialize( expectedResult),JsonSerializer.Serialize(actualResult.result));
        Assert.Equal(expectedResult,actualResult.result);
    }

    [InlineData(2)]
    [Theory]
    public async void GetEmployeeCheck(int id)
    {
        // Arrange
        var employees = this.GetEmployeeList();
        employeeRepository.Setup(x => x.Get(id)).ReturnsAsync(employees.FirstOrDefault(employee => employee.Id == id));
        var expectedResult = this.GetEmployeeDtoList().FirstOrDefault(employee => employee.Id == id);
        httpContext.Setup(h => h.HttpContext.User).Returns(employeeUser);

        // Act
        var actionResult = await employeeController.GetEmployee(id);
        
        // Assert 
        Assert.IsType<OkObjectResult>(actionResult);
        Assert.NotNull(actionResult);
        
        var okObjectResult =actionResult as OkObjectResult;
        var actualResult =okObjectResult!.Value as ResponseDto;
        Assert.NotNull(actualResult);
        Assert.Equal(Resource.EmployeeFetched,actualResult!.message);
        Assert.Equal(expectedResult,actualResult.result);
        
    }
    
    [InlineData(3)]
    [Theory]
    public async void GetEmployeeCheck_When_Authorized(int id)
    {
        // Arrange
        var employees = GetEmployeeList();
        employeeRepository.Setup(x => x.Get(id)).ReturnsAsync(employees.FirstOrDefault(employee => employee.Id == id));
        var expectedResult = this.GetEmployeeDtoList().FirstOrDefault(employee => employee.Id == id);
        httpContext.Setup(h => h.HttpContext.User).Returns(employeeUser);

        // Act
        var actionResult = await Assert.ThrowsAsync<UnauthorizedAccessException>(()=> employeeController.GetEmployee(id));

       
        // // Assert 
        Assert.IsType<UnauthorizedAccessException>(actionResult);

    }
    
    private IEnumerable<Employee> GetEmployeeList()
    {
        var employees = new List<Employee>
        {
            new Employee
            {
                Id = 2,
                Name = "Ajay",
                Email = "ajayr@presidio.com",
                Password = "$2a$12$yRa0uM7QZLtH69MgaZ9LNOyZOFFLittksNqzIUWRKV06iPjh0M/pq",
                Role = Role.Employee,
                ManagerId = 4,
                Manager = null,
                Requests = null
            },
            new Employee
            {
                Id = 3,
                Name = "Ujwala",
                Email = "ujwala@presidio.com",
                Password = "$2a$12$yRa0uM7QZLtH69MgaZ9LNOyZOFFLittksNqzIUWRKV06iPjh0M/pq",
                Role = Role.Admin,
                ManagerId = 4,
                Manager = null,
                Requests = null
            }
        };

        IEnumerable<Employee> employeeList = employees;
        return employeeList;

    }

    private List<EmployeeResponseDto> GetEmployeeDtoList()
    {
        var employees = new List<EmployeeResponseDto>()
        {
            new EmployeeResponseDto
            {
                Id = 2,
                Name = "Ajay",
                Email = "ajayr@presidio.com",
                Role = Role.Employee,
                ManagerId = 4
            },
            new EmployeeResponseDto()
            {
                Id = 3,
                Name = "Ujwala",
                Email = "ujwala@presidio.com",
                Role = Role.Admin,
                ManagerId = 4
            }
        };
        return employees;
    }

    private static ClaimsPrincipal employeeUser = new ClaimsPrincipal(
        new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "2"),
            new Claim(ClaimTypes.Name, "Ajay"),
            new Claim(ClaimTypes.Email, "ajayr@presidio.com"),
            new Claim(ClaimTypes.Role, Role.Employee.ToString()),

        }));
    
    private static ClaimsPrincipal adminUser = new ClaimsPrincipal(
        new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "3"),
            new Claim(ClaimTypes.Name, "Ujwala"),
            new Claim(ClaimTypes.Email, "ujwala@presidio.com"),
            new Claim(ClaimTypes.Role, Role.Admin.ToString()),

        }));

}