using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Quickstart.Home;

[SecurityHeaders]
[AllowAnonymous]
public class HomeController : Controller
{
    private readonly IWebHostEnvironment _environment;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ILogger _logger;

    public HomeController(IWebHostEnvironment environment, IIdentityServerInteractionService interaction, ILogger logger)
    {
        _environment = environment;
        _interaction = interaction;
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (_environment.IsDevelopment())
        {
            return View();
        }

        _logger.LogInformation("Homepage is disabled in production. Returning 404.");

        return NotFound();
    }

    public async Task<IActionResult> Error(string errorId)
    {
        var vm = new ErrorViewModel();

        var message = await _interaction.GetErrorContextAsync(errorId);

        if (message != null)
        {
            vm.Error = message;

            if (!_environment.IsDevelopment())
            {
                message.ErrorDescription = null;
            }
        }

        return View("Error", vm);
    }
}
