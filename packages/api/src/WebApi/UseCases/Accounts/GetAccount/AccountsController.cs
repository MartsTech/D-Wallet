namespace WebApi.UseCases.Accounts.GetAccount;

using Application.UseCases.Accounts.GetAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApi.Modules.Common.FeatureFlags;

[FeatureGate(CustomFeature.GetAccount)]
public sealed class AccountsController : BaseApiController
{
    [Authorize]
    [HttpGet("{accountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAccountResponse))]
    public async Task<IActionResult> GetAccount(
        [FromRoute][Required] Guid accountId)
    {
        GetAccountInput input = new() 
        { 
            AccountId = accountId 
        };

        return HandleResult(await Mediator.Send(new GetAccountUseCase.Query(input)));
    }
}