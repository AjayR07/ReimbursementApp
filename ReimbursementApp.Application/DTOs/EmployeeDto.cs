using System.Text.Json.Serialization;
using ReimbursementApp.Domain.Enums;

namespace ReimbursementApp.Application.DTOs;

public class EmployeeDto
{
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public Role? Role { get; set; }

    public int? ManagerId { get; set; }
}