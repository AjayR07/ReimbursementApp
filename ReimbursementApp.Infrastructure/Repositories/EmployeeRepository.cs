using ReimbursementApp.Domain.Models;
using ReimbursementApp.Infrastructure.Interfaces;

namespace ReimbursementApp.Infrastructure.Repositories;

public class EmployeeRepository : GenericRepository<Employee>,IEmployeeRepository
{
    public EmployeeRepository(ReimburseContext context) : base(context)
    {
    }
}