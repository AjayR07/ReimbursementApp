using FluentValidation;
using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.Application.Validators;

public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeDtoValidator()
    {
        RuleFor(employee => employee.Name).NotNull().MinimumLength(3).WithMessage("Employee Name must have 3 characters");
        RuleFor(employee => employee.Email).NotNull().EmailAddress();
        RuleFor(employee => employee.Password).NotNull().Length(8,16).WithMessage("Password length must be between 8 and 16 characters");
        RuleFor(employee => employee.Role).IsInEnum().WithMessage("Enter a valid role");
    }
}