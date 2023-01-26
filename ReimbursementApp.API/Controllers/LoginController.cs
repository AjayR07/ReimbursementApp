using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Application.Interfaces;
using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.API.Controllers;

[ApiController]
[Route("api/authenticate")]
public class LoginController: ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILoginService _loginService;

    public LoginController(IMapper mapper, ILoginService loginService)
    {
        _mapper = mapper;
        _loginService = loginService;
    }
    
    [HttpPost]
    public async Task<ActionResult<TokenDto>> Authenticate([FromBody] LoginDto loginDto)
    {
        var login = _mapper.Map<Login>(loginDto);
        var response = _mapper.Map<TokenDto>(await _loginService.Authenticate(login));
        return Ok(response);
    }
}