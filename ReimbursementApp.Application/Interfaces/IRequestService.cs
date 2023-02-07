using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Domain.Enums;
using ReimbursementApp.Domain.Models;

namespace ReimbursementApp.Application.Interfaces;

public interface IRequestService
{
    Task<ReimbursementRequest> RaiseRequest(ReimbursementRequestDto request);

    Task<List<ReimbursementRequest>> GetAllMyRequest();

    Task<ReimbursementRequest> GetRequest(int id);

    Task<List<ReimbursementRequest>> GetPendingRequests();
    Task<ReimbursementRequest> Acknowledge(int id, ApprovalStatus status);
}