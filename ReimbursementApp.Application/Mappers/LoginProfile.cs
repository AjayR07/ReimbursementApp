using AutoMapper;
using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.Application.Mappers;

public class LoginProfile: Profile
{
    public LoginProfile()
    {
        CreateMap<Login, LoginDto>().ReverseMap();
    }
}