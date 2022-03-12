namespace WebApi.UseCases.Accounts.GetAccounts;

using Application.UseCases.Accounts.GetAccounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using WebApi.Modules.Common.FeatureFlags;

[FeatureGate(CustomFeature.GetAccounts)]
public sealed class AccountsController : BaseApiController
{
    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAccountsResponse))]
    public async Task<IActionResult> GetAccounts()
    {
        return HandleResult(await Mediator.Send(new GetAccountsUseCase.Query()));
    }
}