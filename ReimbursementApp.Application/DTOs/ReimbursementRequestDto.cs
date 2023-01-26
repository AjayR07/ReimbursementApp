using Microsoft.AspNetCore.Http;

namespace ReimbursementApp.Application.DTOs;

public class ReimbursementRequestDto
{
    public string Description { get; set; } = string.Empty;

    public IFormFile Bill { get; set; }
}