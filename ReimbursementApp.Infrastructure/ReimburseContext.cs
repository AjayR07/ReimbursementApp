using Microsoft.EntityFrameworkCore;
using ReimbursementApp.Domain.Enums;
using ReimbursementApp.Domain.Models;
using ReimbursementApp.Infrastructure.Interfaces;

namespace ReimbursementApp.Infrastructure;

public class ReimburseContext:DbContext, IReimburseContext
{
    public ReimburseContext():base()
    {
        
    }
    public ReimburseContext(DbContextOptions<ReimburseContext> options):base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Password)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(prop => prop.Role).HasDefaultValue(Role.Employee);
        });
        modelBuilder.Entity<ReimbursementRequest>(entity =>
        {
            entity.HasKey(prop => prop.Id);
            entity.Property(prop => prop.AdminApprovalStatus).HasDefaultValue(ApprovalStatus.WaitingForApproval);
        });
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<ReimbursementRequest> Requests { get; set; }
}