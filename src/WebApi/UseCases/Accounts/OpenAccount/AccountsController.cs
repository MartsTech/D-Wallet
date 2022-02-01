namespace WebApi.UseCases.Accounts.OpenAccount;

using System.ComponentModel.DataAnnotations;
using Application.Services;
using Application.UseCases.OpenAccount;
using Domain.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using WebApi.Modules.Common;
using WebApi.Modules.Common.FeatureFlags;
using WebApi.ViewModels;

[ApiVersion("1.0")]
[FeatureGate(CustomFeature.OpenAccount)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public sealed class AccountsController : ControllerBase, IOutputPort
{
    private readonly Notification _notification;

    private IActionResult? _viewModel;

    public AccountsController(Notification notification)
    {
        _notification = notification;
    }

    void IOutputPort.Invalid()
    {
        ValidationProblemDetails problemDetails = new(_notification.ModelState);
        _viewModel = BadRequest(problemDetails);
    }

    void IOutputPort.NotFound()
    {
        _viewModel = NotFound();
    }

    void IOutputPort.Ok(Account account)
    {
        _viewModel = Ok(new OpenAccountResponse(new AccountModel(account)));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OpenAccountResponse))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OpenAccountResponse))]
    [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Post))]
    public async Task<IActionResult> Post(
        [FromServices] IOpenAccountUseCase useCase,
        [FromForm][Required] decimal amount,
        [FromForm][Required] string currency)
    {
        useCase.SetOutputPort(this);

        await useCase
            .Execute(amount, currency)
            .ConfigureAwait(false);

        return _viewModel!;
    }
}