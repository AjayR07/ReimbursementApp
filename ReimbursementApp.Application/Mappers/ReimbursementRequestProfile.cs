using AutoMapper;
using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.Application.Mappers;

public class ReimbursementRequestProfile: Profile
{
    public ReimbursementRequestProfile()
    {
        CreateMap<ReimbursementRequest, ReimbursementRequestDto>().ReverseMap();
    }
}