using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReimbursementApp.Domain.Enums;

namespace ReimbursementApp.Domain.Models;

public class Employee
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key, Column(Order = 0)]
    public int Id { get; set; }
    
    [Required]
    [MinLength(3)]
    public string Name { get; set; } = string.Empty;

    [Required] 
    [EmailAddress] 
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [MaxLength(16)]
    [PasswordPropertyText]
    public string Password { get; set; } = string.Empty;
    
    public Role Role { get; set; }
    

    [ForeignKey("Employee")]
    public int? ManagerId { get; set; }
    
    public virtual Employee? Manager { get; set; }
    
    public virtual ICollection<ReimbursementRequest> Requests { get; set; }
}