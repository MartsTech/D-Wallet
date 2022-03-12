namespace WebApi.UseCases.Transactions.Transfer;

using Application.UseCases.Transactions.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Modules.Common.FeatureFlags;

[FeatureGate(CustomFeature.Transfer)]
public sealed class TransactionsController : BaseApiController
{
    [Authorize]
    [HttpPatch("{accountId:guid}/{destinationAccountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransferResponse))]
    public async Task<IActionResult> Transfer(
        [FromRoute][Required] Guid accountId,
        [FromRoute][Required] Guid destinationAccountId,
        [FromForm][Required] decimal amount,
        [FromForm][Required] string currency)
    {
        TransferInput input = new()
        {
            OriginAccountId = accountId,
            DestinationAccountId = destinationAccountId,
            Amount = amount,
            Currency = currency
        };

        return HandleResult(await Mediator.Send(new TransferUseCase.Command(input)));
    }
}