using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReimbursementApp.API.Configurations;
using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Application.Interfaces;
using ReimbursementApp.Domain.Constants;
using ReimbursementApp.Domain.Enums;

namespace ReimbursementApp.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize]
[AuthorizeRoles(Role.Admin)]
public class AdminController:ControllerBase
{
    private readonly IRequestService _requestService;
    private readonly IMapper _mapper;

    public AdminController(IRequestService requestService,IMapper mapper)
    {
        _requestService = requestService;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("request/pending")]
    public async Task<ActionResult> GetAllPendingRequests()
    {
        var requests = await _requestService.GetAdminApprovalPendingRequests();
        var response = new
        {
            message = RequestConstants.PendingRequestsFetched,
            result = requests.Select(req => _mapper.Map<ReimbursementResponeDto>(req))
        };
        return new OkObjectResult(response);
    }

    [HttpPut]
    [Route("acknowledge")]
    public async Task<ActionResult> AdminAcknowledge(int RequestId, ApprovalStatus status)
    {
        var request = await _requestService.AdminAcknowlege(RequestId,status);
        var response = new
        {
            message = RequestConstants.AdminAcknowledged,
            result = _mapper.Map<ReimbursementResponeDto>(request)
        };
        return new OkObjectResult(response);
    }
    
    
}