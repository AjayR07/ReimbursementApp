using ReimbursementApp.Domain.Enums;
using ReimbursementApp.Domain.Models;
using ReimbursementApp.Infrastructure.Interfaces;

namespace ReimbursementApp.Infrastructure.Repositories;

public class ReimbursementRequestRepository: GenericRepository<ReimbursementRequest>, IReimbursementRequestRepository
{
    private readonly ReimburseContext _context;

    public ReimbursementRequestRepository(ReimburseContext context) : base(context)
    {
        _context = context;
    }

    public IQueryable<ReimbursementRequest> GetAllMyRequest(int id)
    {
        return  _context.Requests.Where(req => req.EmployeeId == id);
    }
    
    public IQueryable<ReimbursementRequest> GetPendingManageeRequests(int id)
    {

        var managees =_context.Employees.Where(emp => emp.ManagerId == id).Select(emp => emp.Id);
        return _context.Requests.Where(req =>
            managees.Contains(req.EmployeeId) && req.AdminApprovalStatus == ApprovalStatus.Approved &&
            req.ManagerApprovalStatus == ApprovalStatus.WaitingForApproval);
    }
}