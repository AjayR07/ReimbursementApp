using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReimbursementApp.Application.Interfaces;
using ReimbursementApp.Domain.Models;
using ReimbursementApp.Infrastructure.Interfaces;

namespace ReimbursementApp.Application.Services;

public class LoginService:ILoginService
{
 private readonly IEmployeeRepository _employeeRepository;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;

    public LoginService(IEmployeeRepository employeeRepository,IConfiguration configuration, IMemoryCache memoryCache)
    {
        _employeeRepository = employeeRepository;
        _configuration = configuration;
        _memoryCache = memoryCache;
    }
    public Token Authenticate(Login login)
    {
 
        var cacheOutput = _memoryCache.Get<LoginCache>(login.EmployeeId);
        if (cacheOutput is not null)
        {
            if (BCrypt.Net.BCrypt.Verify(login.Password, cacheOutput.Password))
            {
                return cacheOutput.Token;
            }
        }
            
        var employee = _employeeRepository.Get(login.EmployeeId);
        if (employee != null)
        {
            if (BCrypt.Net.BCrypt.Verify(login.Password, employee.Password))
            {
               
                var cache = new LoginCache()
                {
                    Password = employee.Password, 
                    Token = new Token()
                    {
                        SecurityToken = GenerateToken(employee)
                    }
                };
                
                // Set cache options
                var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(int.Parse(_configuration["JWT:Duration"])));
                // Set object in cache
                _memoryCache.Set(login.EmployeeId,cache);
                return cache.Token;
            }
            else
            {
                // throw Invalid password
                return null;
            }
        }
        else
        {
            //throw Invalid Employee ID
            return null;
        }
      
    }

    private string GenerateToken(Employee employee)
    {
        //Generate JSON Web Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, employee.Name)
            }),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JWT:Duration"])),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

class LoginCache
{
    public string Password { get; set; }
    
    public Token Token { get; set; }
}