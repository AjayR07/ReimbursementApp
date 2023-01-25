using FluentValidation;
using ReimbursementApp.Application.DTOs;

namespace ReimbursementApp.Application.Validators;

public class ReimbursementRequestDtoValidator: AbstractValidator<ReimbursementRequestDto>
{
    public ReimbursementRequestDtoValidator()
    {
        RuleFor(request => request.EmployeeId).NotNull().WithMessage("Employee Id is mandatory");
        RuleFor(request => request.BillUrl).NotNull().WithMessage("Bill Url is Mandatory");
        RuleFor(request => request.Description).NotNull().WithMessage("Description is required");
        RuleFor(request => request.AdminApprovalStatus).IsInEnum();
        RuleFor(request => request.ManagerApprovalStatus).IsInEnum();
    }
}