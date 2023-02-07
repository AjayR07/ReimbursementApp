using Microsoft.AspNetCore.Mvc;

namespace ReimbursementApp.API.Controllers.v2;
[ApiController]
[Route("")]
[ApiVersion("2.0")]
public class HomeController : ControllerBase
{
    [HttpGet]
    [MapToApiVersion("2.0")]
    public ActionResult<string> Get()
    {
        return Ok("Welcome to Reimbursement System V2.0");
    }
}