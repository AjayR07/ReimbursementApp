
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReimbursementApp.Application.DTOs;
using ReimbursementApp.Application.Interfaces;
using ReimbursementApp.Domain.Constants;

namespace ReimbursementApp.API.Controllers;

[ApiController]
[Authorize]
[Route("api/request")]
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
        var response = new
            { message = RequestConstants.RequestRaised,
                result = _mapper.Map<ReimbursementResponeDto>(request) };
        return new OkObjectResult(response);
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult> GetMyRequests()
    {
        var requests = await _requestService.GetAllMyRequest();
        var response = new
        {
            message = RequestConstants.RequestsFetched,
            result = requests.Select(req => _mapper.Map<ReimbursementResponeDto>(req))
        };
        return new OkObjectResult(response);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetRequest(int id)
    {
        var request = await _requestService.GetRequest(id);
        var response = new
        {
            message = RequestConstants.RequestsFetched,
            result = _mapper.Map<ReimbursementResponeDto>(request)
        };
        return new OkObjectResult(response);
    }
}