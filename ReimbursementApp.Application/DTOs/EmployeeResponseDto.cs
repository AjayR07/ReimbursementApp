using System.ComponentModel.DataAnnotations;
using ReimbursementApp.Domain.Enums;

namespace ReimbursementApp.Application.DTOs;

public class EmployeeResponseDto
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public Role Role { get; set; }

    public int? ManagerId { get; set; }
    
    // public  EmployeeDto? Manager { get; set; }
    //
    // public  ICollection<ReimbursementRequestDto> Requests { get; set; }
}