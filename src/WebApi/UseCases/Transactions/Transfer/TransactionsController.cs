namespace WebApi.UseCases.Transactions.Transfer;

using System.ComponentModel.DataAnnotations;
using Application.Services;
using Application.UseCases.Transfer;
using Domain.Accounts;
using Domain.Credits;
using Domain.Debits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using WebApi.Modules.Common;
using WebApi.Modules.Common.FeatureFlags;
using WebApi.ViewModels;

[FeatureGate(CustomFeature.Transfer)]
[ApiVersion("1.0")]
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
        _viewModel = BadRequest("Out of funds.");
    }

    void IOutputPort.Ok(Account originAccount, Debit debit, Account destinationAccount, Credit credit)
    {
        _viewModel = Ok(new TransferResponse(new DebitModel(debit)));
    }

    [Authorize]
    [HttpPatch("{accountId:guid}/{destinationAccountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransferResponse))]
    [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Patch))]
    public async Task<IActionResult> Transfer(
        [FromServices] ITransferUseCase useCase,
        [FromRoute][Required] Guid accountId,
        [FromRoute][Required] Guid destinationAccountId,
        [FromForm][Required] decimal amount,
        [FromForm][Required] string currency)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(
                accountId,
                destinationAccountId,
                amount,
                currency)
            .ConfigureAwait(false);

        return _viewModel!;
    }
}

