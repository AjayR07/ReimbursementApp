using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.Application.Interfaces;

public interface ILoginService
{
    Task<Token> Authenticate(Login login);
}