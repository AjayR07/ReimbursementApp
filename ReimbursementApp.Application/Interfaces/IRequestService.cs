using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Domain.Enums;
using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.Application.Interfaces;

public interface IRequestService
{
    Task<ReimbursementRequest> RaiseRequest(ReimbursementRequestDto request);

    Task<List<ReimbursementRequest>> GetAllMyRequest();

    Task<ReimbursementRequest> GetRequest(int id);

    Task<List<ReimbursementRequest>> GetAdminApprovalPendingRequests();

    Task<ReimbursementRequest> AdminAcknowlege(int id, ApprovalStatus status);
    IEnumerable<ReimbursementRequest> GetManagerApprovalPendingRequests();
    Task<ReimbursementRequest> ManagerAcknowlege(int requestId, ApprovalStatus status);
}