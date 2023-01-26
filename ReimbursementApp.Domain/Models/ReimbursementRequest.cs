using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using ReimbursementApp.Domain.Enums;

namespace ReimbursementApp.Domain.Models;

public class ReimbursementRequest
{
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }
    
    public virtual Employee Employee { get; set; }
  
    public DateTime RequestDate { get; set; }
    
    [Required]
    [MinLength(3)]
    [StringLength(256)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public string BillUrl { get; set;  } = string.Empty;
    
    [AllowNull]
    public ApprovalStatus AdminApprovalStatus { get; set; }
    
    [AllowNull]
    public ApprovalStatus ManagerApprovalStatus { get; set; }

    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(EmployeeId)}: {EmployeeId}, {nameof(Employee)}: {Employee}, {nameof(RequestDate)}: {RequestDate}, {nameof(Description)}: {Description}, {nameof(BillUrl)}: {BillUrl}, {nameof(AdminApprovalStatus)}: {AdminApprovalStatus}, {nameof(ManagerApprovalStatus)}: {ManagerApprovalStatus}";
    }
}
 
