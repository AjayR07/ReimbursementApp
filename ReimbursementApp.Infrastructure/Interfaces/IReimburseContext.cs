using Microsoft.EntityFrameworkCore;
using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.Infrastructure.Interfaces;

public interface IReimburseContext
{
    DbSet<Employee> Employees { get; set; }

    DbSet<ReimbursementRequest> Requests { get; set; }
}