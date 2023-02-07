using Microsoft.AspNetCore.Mvc;

namespace ReimbursementApp.API.Controllers.v1;
[ApiController]
[Route("")]
[ApiVersion("1.0")]
public class HomeController : ControllerBase
{
    [HttpGet]
    [MapToApiVersion("1.0")]
    public ActionResult<string> Get()
    {
        return Ok("Welcome to Reimbursement System");
    }
}