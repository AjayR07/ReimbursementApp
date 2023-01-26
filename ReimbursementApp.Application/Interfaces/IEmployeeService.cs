using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<Employee>> GetAllEmployees();

    Task<Employee?> GetEmployeeById(int id);

    Task<Employee> AddNewEmployee(Employee employee);

    Task<Employee?> UpdateEmployee(Employee employee);
    
    Task RemoveEmployee(int id);
}