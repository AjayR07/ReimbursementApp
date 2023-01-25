using Microsoft.AspNetCore.Mvc;

namespace ReimbursementApp.API.Controllers;
[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Get()
    {
        return Ok("Welcome to Reimbursement System");
    }
}