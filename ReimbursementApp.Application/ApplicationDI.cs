using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReimbursementApp.Application.Interfaces;
using ReimbursementApp.Application.Services;

namespace ReimbursementApp.Application;

public static class ApplicationDI
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configurations)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ILoginService, LoginService>();
        return services;
    }
}