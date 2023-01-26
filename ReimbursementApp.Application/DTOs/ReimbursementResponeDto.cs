using ReimbursementApp.Domain.Enums;

namespace ReimbursementApp.Application.DTOs;

public class ReimbursementResponeDto
{
    
    public int Id { get; set; }
    
    public int EmployeeId { get; set; }
    
    public DateTime RequestDate { get; set; }
    
    public string Description { get; set; } = string.Empty;
    
    public string BillUrl { get; set;  } = string.Empty;
    
    public string AdminApprovalStatus { get; set; }
    
    
    public string ManagerApprovalStatus { get; set; }
}