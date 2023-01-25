namespace ReimbursementApp.Domain.Models;

public class Token
{
    public string SecurityToken { get; set; } = string.Empty;
    
    public string RefreshToken { get; set; } = string.Empty;
}