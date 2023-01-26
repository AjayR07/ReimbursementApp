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
[Route("api/manager")]
[Authorize]
[AuthorizeRoles(Role.Manager)]
public class ManagerController: ControllerBase
{
    private readonly IRequestService _requestService;
    private readonly IMapper _mapper;

    public ManagerController(IRequestService requestService,IMapper mapper)
    {
        _requestService = requestService;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("request/pending")]
    public ActionResult GetAllPendingRequests()
    {
        var requests =  _requestService.GetManagerApprovalPendingRequests();
        var response = new
        {
            message = RequestConstants.PendingRequestsFetched,
            result = requests.Select(req => _mapper.Map<ReimbursementResponeDto>(req))
        };
        return new OkObjectResult(response);
    }

    [HttpPut]
    [Route("acknowledge")]
    public async Task<ActionResult> ManagerAcknowledge(int RequestId, ApprovalStatus status)
    {
        var request = await _requestService.ManagerAcknowlege(RequestId,status);
        var response = new
        {
            message = RequestConstants.ManagerAcknowledged,
            result = _mapper.Map<ReimbursementResponeDto>(request)
        };
        return new OkObjectResult(response);
    }
}