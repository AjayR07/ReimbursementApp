using ReimbursementApp.Application.Interfaces;
using ReimbursementApp.Domain.Models;
using ReimbursementApp.Infrastructure.Interfaces;

namespace ReimbursementApp.Application.Services;

public class EmployeeService: IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    
    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
   
    }
    public IEnumerable<Employee> GetAllEmployees()
    {
        return _employeeRepository.GetAll();
    }

    public Employee? GetEmployeeById(int id)
    {
        return _employeeRepository.Get(id);
    }

    public Employee AddNewEmployee(Employee employee)
    {
        employee.Password = BCrypt.Net.BCrypt.HashPassword(employee.Password);
        return _employeeRepository.Add(employee);
    }

    public void RemoveEmployee(int id)
    {
        var employee = GetEmployeeById(id);
        if (employee != null)
        {
            _employeeRepository.Delete(employee);
        }
    }

    public void UpdateEmployee(Employee updatedEmployee)
    {
        var employee = GetEmployeeById(updatedEmployee.Id);
        if (BCrypt.Net.BCrypt.Verify(updatedEmployee.Password, employee.Password))
            updatedEmployee.Password = employee.Password;
        else
            updatedEmployee.Password =  BCrypt.Net.BCrypt.HashPassword(updatedEmployee.Password);
        _employeeRepository.Update(updatedEmployee);
    }
}