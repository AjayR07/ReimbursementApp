using AutoMapper;
using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.Application.Mappers;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<EmployeeDto, Employee>();
        CreateMap<EmployeeUpdateRequestDto, Employee>();
        CreateMap<Employee, EmployeeResponseDto>();
        
    }
}