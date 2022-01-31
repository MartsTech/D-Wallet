namespace WebApi.UseCases.CustomError;

using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class CustomErrorController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult CustomError()
    {
        CustomErrorResponse model = new()
        { 
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
        };

        return View("~/UseCases/CustomError/CustomError.cshtml", model);
    }
}
