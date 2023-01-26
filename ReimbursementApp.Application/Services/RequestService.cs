using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Application.Interfaces;
using ReimbursementApp.Domain.Constants;
using ReimbursementApp.Domain.Enums;
using ReimbursementApp.Domain.Models;
using ReimbursementApp.Infrastructure.Interfaces;
using UnauthorizedAccessException = ReimbursementApp.Application.Exceptions.UnauthorizedAccessException;

namespace ReimbursementApp.Application.Services;

public class RequestService: IRequestService
{
    private readonly IReimbursementRequestRepository _reimbursementRequestRepository;

    private readonly IAzureStorage _azureStorage;
    private readonly IServiceBus _serviceBus;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContext;

    public RequestService(IReimbursementRequestRepository reimbursementRequestRepository,
        IHttpContextAccessor httpContext, IAzureStorage azureStorage, IServiceBus serviceBus,
        IConfiguration configuration
    )
    {
        _reimbursementRequestRepository = reimbursementRequestRepository;
        _httpContext = httpContext;
        _azureStorage = azureStorage;
        _serviceBus = serviceBus;
        _configuration = configuration;
    }

    public async Task<ReimbursementRequest> RaiseRequest(ReimbursementRequestDto request)
    {
        if (_httpContext.HttpContext == null)
            throw new UnauthorizedAccessException(AuthenticationConstants.TokenInvalid);
        var bill = await _azureStorage.UploadAsync(request.Bill);
        if (bill.Error || bill.Blob.Uri == null)
            throw new Exception(AzureConstants.FileUploadFailed+" : "+ bill.Status);
        
        var req = new ReimbursementRequest
        {
            EmployeeId = int.Parse(_httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)), 
            RequestDate = DateTime.Now, 
            Description = request.Description, 
            BillUrl = bill.Blob.Uri
        };
        var result = await _reimbursementRequestRepository.Add(req);
        await _serviceBus.SendMessageAsync(result,_configuration["AdminQueueName"]);
        return result;
    }

    public Task<List<ReimbursementRequest>> GetAllMyRequest()
    {
       return _reimbursementRequestRepository.GetAllMyRequest(
            int.Parse(_httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier))).ToListAsync();
    }
    
    public async Task<ReimbursementRequest> GetRequest(int id)
    {
        var request = await _reimbursementRequestRepository.Get(id);
        if(int.Parse(_httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier))!= request.EmployeeId && _httpContext.HttpContext.User.IsInRole(Role.Employee.ToString())) 
            throw new UnauthorizedAccessException(AuthenticationConstants.UnAuthorized);
        return request;
    }

    public async Task<List<ReimbursementRequest>> GetAdminApprovalPendingRequests()
    {
        var messages =await _serviceBus.ReceiveMessagesAsync(_configuration["AdminQueueName"]);
        return messages;

    }

    public async Task<ReimbursementRequest> AdminAcknowlege(int id, ApprovalStatus status)
    {
        var request = await this.GetRequest(id);
        request.AdminApprovalStatus = status;
        var result = await _reimbursementRequestRepository.Update(request); 
        await _serviceBus.RemoveMessageFromQueue(_configuration["AdminQueueName"],id);
        return result;
    }

    public IEnumerable<ReimbursementRequest> GetManagerApprovalPendingRequests()
    {
        var managerId = int.Parse(_httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));
        return _reimbursementRequestRepository.GetPendingManageeRequests(managerId);
    }

    public async Task<ReimbursementRequest> ManagerAcknowlege(int id, ApprovalStatus status)
    {
        var request = await this.GetRequest(id);
        request.ManagerApprovalStatus = status;
        var result = await _reimbursementRequestRepository.Update(request); 
        return result;
    }
}