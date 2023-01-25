using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.Application.Interfaces;

public interface ILoginService
{
    Token Authenticate(Login login);
}