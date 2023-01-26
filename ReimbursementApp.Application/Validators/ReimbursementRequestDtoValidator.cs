using FluentValidation;
using ReimbursementApp.Application.DTOs;

namespace ReimbursementApp.Application.Validators;

public class ReimbursementRequestDtoValidator: AbstractValidator<ReimbursementRequestDto>
{
    public ReimbursementRequestDtoValidator()
    {
        RuleFor(request => request.Bill).NotNull().WithMessage("Bill is mandatory for filing a request");
        RuleFor(request => request.Description).NotNull().WithMessage("Description is required");
    }
}