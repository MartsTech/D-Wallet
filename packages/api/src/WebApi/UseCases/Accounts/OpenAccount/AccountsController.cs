namespace WebApi.UseCases.Accounts.OpenAccount;

using Application.UseCases.Accounts.OpenAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using WebApi.Modules.Common.FeatureFlags;

[FeatureGate(CustomFeature.OpenAccount)]
public sealed class AccountsController : BaseApiController
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OpenAccountResponse))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OpenAccountResponse))]
    public async Task<IActionResult> OpenAccount(
        [FromForm] OpenAccountInput input)
    {
        return HandleResult(await Mediator.Send(new OpenAccountUseCase.Command(input)));
    }
}