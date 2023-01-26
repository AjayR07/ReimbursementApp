using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReimbursementApp.Application.Exceptions;
using ReimbursementApp.Application.Interfaces;
using ReimbursementApp.Domain.Constants;
using ReimbursementApp.Domain.Models;
using ReimbursementApp.Infrastructure.Interfaces;
using NotFoundException = ReimbursementApp.Application.Exceptions.NotFoundException;
namespace ReimbursementApp.Application.Services;

public class LoginService:ILoginService
{
 private readonly IEmployeeRepository _employeeRepository;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;
    private readonly IMapper _mapper;

    public LoginService(IEmployeeRepository employeeRepository,IConfiguration configuration, IMemoryCache memoryCache,IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _configuration = configuration;
        _memoryCache = memoryCache;
        _mapper = mapper;
    }
    public async Task<Token> Authenticate(Login login)
    {
 
        var cacheOutput = _memoryCache.Get<LoginCache>(login.EmployeeId);
        if (cacheOutput is not null)
        {
            if (BCrypt.Net.BCrypt.Verify(login.Password, cacheOutput.Password))
            {
                return cacheOutput.Token;
            }
        }
            
        var employee =await _employeeRepository.Get(login.EmployeeId);
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
                var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(int.Parse(_configuration["JWT:Expiry"])));
                // Set object in cache
                _memoryCache.Set(login.EmployeeId,cache);
                return cache.Token;
            }
            else
            {
                // throw Invalid password
                throw new PasswordMismatchException(EmployeeConstants.EmployeePasswordMismatch);
            }
        }
        else
        {
            throw new NotFoundException(EmployeeConstants.EmployeeNotFound);
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
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim(ClaimTypes.Name, employee.Name),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.Role, employee.Role.ToString()),
               
            }),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JWT:Expiry"])),
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