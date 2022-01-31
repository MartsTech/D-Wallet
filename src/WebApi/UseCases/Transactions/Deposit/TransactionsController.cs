namespace WebApi.UseCases.Transactions.Deposit;

using System.ComponentModel.DataAnnotations;
using Application.Services;
using Application.UseCases.Deposit;
using Domain.Accounts;
using Domain.Credits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using WebApi.Modules.Common;
using WebApi.Modules.Common.FeatureFlags;
using WebApi.ViewModels;

[ApiVersion("1.0")]
[FeatureGate(CustomFeature.Deposit)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public sealed class TransactionsController : ControllerBase, IOutputPort
{
    private readonly Notification _notification;

    private IActionResult? _viewModel;

    public TransactionsController(Notification notification)
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

    void IOutputPort.Ok(Credit credit, Account account)
    {
        _viewModel = Ok(new DepositResponse(new CreditModel(credit)));
    }

    [Authorize]
    [HttpPatch("{accountId:guid}/Deposit")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DepositResponse))]
    [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Patch))]
    public async Task<IActionResult> Deposit(
        [FromServices] IDepositUseCase useCase,
        [FromRoute][Required] Guid accountId,
        [FromForm][Required] decimal amount,
        [FromForm][Required] string currency)
    {
        useCase.SetOutputPort(this);

        await useCase
            .Execute(accountId, amount, currency)
            .ConfigureAwait(false);

        return _viewModel!;
    }
}
