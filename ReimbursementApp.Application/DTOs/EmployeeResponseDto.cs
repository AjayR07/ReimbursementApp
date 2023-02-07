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
    protected bool Equals(EmployeeResponseDto other)
    {
        return Id == other.Id && Name == other.Name && Email == other.Email && Role == other.Role && ManagerId == other.ManagerId;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((EmployeeResponseDto)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Email, (int)Role, ManagerId);
    }
}