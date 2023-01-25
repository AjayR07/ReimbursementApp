using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReimbursementApp.Infrastructure.Interfaces;
using ReimbursementApp.Infrastructure.Repositories;

namespace ReimbursementApp.Infrastructure;

public static class InfrastructureDI
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReimburseContext>(opt=>
            {
                opt.UseSqlServer(configuration.GetConnectionString("SSMS"),
                    b => b.MigrationsAssembly(typeof(ReimburseContext).Assembly.FullName));
                opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, 
            ServiceLifetime.Transient);
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IReimbursementRequestRepository, ReimbursementRequestRepository>();
        return services;
    }
}