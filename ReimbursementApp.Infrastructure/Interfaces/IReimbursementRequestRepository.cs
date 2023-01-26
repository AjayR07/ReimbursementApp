using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.Infrastructure.Interfaces;

public interface IReimbursementRequestRepository: IGenericRepository<ReimbursementRequest>
{
    IQueryable<ReimbursementRequest> GetAllMyRequest(int id);

    IQueryable<ReimbursementRequest> GetPendingManageeRequests(int id);
}