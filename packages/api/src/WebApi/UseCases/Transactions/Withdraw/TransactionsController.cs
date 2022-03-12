namespace WebApi.UseCases.Transactions.Withdraw;

using Application.UseCases.Transactions.Withdraw;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Modules.Common.FeatureFlags;

[FeatureGate(CustomFeature.Withdraw)]
public sealed class TransactionsController : BaseApiController
{
    [Authorize]
    [HttpPatch("{accountId:guid}/Withdraw")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WithdrawResponse))]
    public async Task<IActionResult> Withdraw(
        [FromRoute][Required] Guid accountId,
        [FromForm][Required] decimal amount,
        [FromForm][Required] string currency)
    {
        WithdrawInput input = new()
        {
            AccountId = accountId,
            Amount = amount,
            Currency = currency
        };

        return HandleResult(await Mediator.Send(new WithdrawUseCase.Command(input)));
    }
}