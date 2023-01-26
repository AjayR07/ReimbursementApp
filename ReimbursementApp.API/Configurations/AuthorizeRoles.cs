using Microsoft.AspNetCore.Authorization;
using ReimbursementApp.Domain.Enums;

namespace ReimbursementApp.API.Configurations;

public class AuthorizeRoles: AuthorizeAttribute
{
    public AuthorizeRoles(params Role[] roles)
    {
        this.Roles = String.Join(",", roles.Select(role => role.ToString()));
    }
    
}