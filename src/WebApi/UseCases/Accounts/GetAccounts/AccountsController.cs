namespace WebApi.UseCases.Accounts.GetAccounts;

using Application.UseCases.GetAccounts;
using Domain.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using WebApi.Modules.Common;
using WebApi.Modules.Common.FeatureFlags;

[ApiVersion("1.0")]
[FeatureGate(CustomFeature.GetAccounts)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public sealed class AccountsController : ControllerBase, IOutputPort
{
    private readonly IGetAccountsUseCase _useCase;

    private IActionResult? _viewModel;

    public AccountsController(IGetAccountsUseCase useCase)
    {
        _useCase = useCase;
    }

    void IOutputPort.Ok(IList<Account> accounts)
    {
        _viewModel = Ok(new GetAccountsResponse(accounts));
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAccountsResponse))]
    [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.List))]
    public async Task<IActionResult> Get()
    {
        _useCase.SetOutputPort(this);

        await _useCase.Execute().ConfigureAwait(false);

        return _viewModel!;
    }
}
