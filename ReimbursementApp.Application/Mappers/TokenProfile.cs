using AutoMapper;
using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.Application.Mappers;

public class TokenProfile: Profile
{
    public TokenProfile()
    {
        CreateMap<Token, TokenDto>().ReverseMap();
    }
}