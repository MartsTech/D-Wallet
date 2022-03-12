namespace WebApi.UseCases.Transactions.Deposit;

using Application.UseCases.Transactions.Deposit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Modules.Common.FeatureFlags;

[FeatureGate(CustomFeature.Deposit)]
public sealed class TransactionsController : BaseApiController
{
    [Authorize]
    [HttpPatch("{accountId:guid}/Deposit")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DepositResponse))]
    public async Task<IActionResult> Deposit(
        [FromRoute][Required] Guid accountId,
        [FromForm][Required] decimal amount,
        [FromForm][Required] string currency)
    {
        DepositInput input = new()
        {
            AccountId = accountId,
            Amount = amount,
            Currency = currency
        };

        return HandleResult(await Mediator.Send(new DepositUseCase.Command(input)));
    }
}