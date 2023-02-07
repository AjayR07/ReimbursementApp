
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReimbursementApp.API.Configurations;
using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Application.Interfaces;
using ReimbursementApp.Domain.Enums;
using ReimbursementApp.Domain.Resources;

namespace ReimbursementApp.API.Controllers.v2;

[ApiController]
[Authorize]
[Route("[controller]")]
[ApiVersion("2.0")]
public class RequestController: ControllerBase
{
    private readonly IRequestService _requestService;
    private readonly IMapper _mapper;

    public RequestController(IRequestService requestService, IMapper mapper)
    {
        _requestService = requestService;
        _mapper = mapper;
    }
    [HttpPost]
    public async Task<ActionResult> RaiseRequest([FromForm] ReimbursementRequestDto requestDto)
    {
        var request = await _requestService.RaiseRequest(requestDto);
        var response = new ResponseDto
            { message = Resource.RequestRaised,
                result = _mapper.Map<ReimbursementResponeDto>(request) };
        return new OkObjectResult(response);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetRequest(int id)
    {
        var request = await _requestService.GetRequest(id);
        var response = new ResponseDto
        {
            message = Resource.RequestsFetched,
            result = _mapper.Map<ReimbursementResponeDto>(request)
        };
        return new OkObjectResult(response);
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult> GetMyRequests()
    {
        var requests = await _requestService.GetAllMyRequest();
        var response = new ResponseDto
        {
            message = Resource.RequestsFetched,
            result = requests.Select(req => _mapper.Map<ReimbursementResponeDto>(req))
        };
        return new OkObjectResult(response);
    }
    
    
    
    [HttpGet]
    [Route("pending")]
    [AuthorizeRoles(Role.Manager, Role.Admin)]
    public async Task<ActionResult> GetAllPendingRequests()
    {
        var requests = await  _requestService.GetPendingRequests();
        var response = new ResponseDto
        {
            message = Resource.PendingRequestsFetched,
            result = requests.Select(req => _mapper.Map<ReimbursementResponeDto>(req))
        };
        return new OkObjectResult(response);
    }

    [HttpPut]
    [Route("acknowledge")]
    [AuthorizeRoles(Role.Manager, Role.Admin)]
    public async Task<ActionResult> ManagerAcknowledge(int RequestId, ApprovalStatus status)
    {
        var request = await _requestService.Acknowledge(RequestId,status);
        var response = new ResponseDto
        {
            message = User.IsInRole(Role.Admin.ToString())?Resource.AdminAcknowledged:Resource.ManagerAcknowledged,
            result = _mapper.Map<ReimbursementResponeDto>(request)
        };
        return new OkObjectResult(response);
    }
    
}