using ReimbursementApp.Domain.Models;
using ReimbursementApp.Infrastructure.Interfaces;

namespace ReimbursementApp.Infrastructure.Repositories;

public class ReimbursementRequestRepository: GenericRepository<ReimbursementRequest>, IReimbursementRequestRepository
{
    public ReimbursementRequestRepository(ReimburseContext context) : base(context)
    {
    }
}