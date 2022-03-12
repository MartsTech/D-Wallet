namespace WebApi.UseCases.Accounts.CloseAccount;

using Application.UseCases.Accounts.CloseAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Modules.Common.FeatureFlags;

[FeatureGate(CustomFeature.CloseAccount)]
public sealed class AccountsController : BaseApiController
{
    [Authorize]
    [HttpDelete("{accountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CloseAccountResponse))]
    public async Task<IActionResult> OpenAccount(
        [FromRoute][Required] Guid accountId)
    {
        CloseAccountInput input = new() 
        { 
            AccountId = accountId
        };

        return HandleResult(await Mediator.Send(new CloseAccountUseCase.Command(input)));
    }
}