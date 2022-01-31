namespace WebApi.UseCases.Transactions.Withdraw;

using System.ComponentModel.DataAnnotations;
using Application.Services;
using Application.UseCases.Withdraw;
using Domain.Accounts;
using Domain.Debits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using WebApi.Modules.Common;
using WebApi.Modules.Common.FeatureFlags;
using WebApi.ViewModels;

[ApiVersion("1.0")]
[FeatureGate(CustomFeature.Withdraw)]
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

    void IOutputPort.OutOfFunds()
    {
        Dictionary<string, string[]> messages = new() { { "", new[] { "Out of funds." } } };

        ValidationProblemDetails problemDetails = new(messages);

        _viewModel = BadRequest(problemDetails);
    }

    void IOutputPort.Ok(Debit debit, Account account)
    {
        _viewModel = Ok(new WithdrawResponse(new DebitModel(debit)));
    }

    [Authorize]
    [HttpPatch("{accountId:guid}/Withdraw")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WithdrawResponse))]
    [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Patch))]
    public async Task<IActionResult> Withdraw(
        [FromServices] IWithdrawUseCase useCase,
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

