using FluentValidation;
using ReimbursementApp.Application.DTOs;

namespace ReimbursementApp.Application.Validators;

public class LoginDtoValidator: AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(loginDto => loginDto.EmployeeId).NotNull().WithMessage("Employee Id is Required");
   
        RuleFor(loginDto => loginDto.Password).NotNull().WithMessage("Employee Id is Required").Length(8,16).WithMessage("password length must be within 8 and 16");
    }
}