using ReimbursementApp.Domain.Enums;

namespace ReimbursementApp.Application.DTOs;

public class ReimbursementRequestDto
{
    public int Id;
    
    public int EmployeeId { get; set; }
    
    public virtual EmployeeDto Employee { get; set; }
 
    public DateTime RequestDate { get; set; }
    
    public string Description { get; set; } = string.Empty;
    
    public string BillUrl { get; set;  } = string.Empty;
    
    public ApprovalStatus AdminApprovalStatus { get; set; }

    public ApprovalStatus ManagerApprovalStatus { get; set; }
}