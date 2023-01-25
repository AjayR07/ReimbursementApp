using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.Application.Interfaces;

public interface IEmployeeService
{
    IEnumerable<Employee> GetAllEmployees();

    Employee? GetEmployeeById(int id);

    Employee AddNewEmployee(Employee employee);

    void RemoveEmployee(int id);

    void UpdateEmployee(Employee employee);
}