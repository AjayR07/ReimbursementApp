namespace ReimbursementApp.Application.DTOs;

public class TokenDto
{
    public string SecurityToken { get; set; } = string.Empty;
    
    public string RefreshToken { get; set; } = string.Empty;
}