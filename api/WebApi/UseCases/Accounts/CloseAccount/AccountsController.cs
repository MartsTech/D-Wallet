using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.UseCases.Accounts.CloseAccount;

using System.ComponentModel.DataAnnotations;
using Application.Services;
using Application.UseCases.CloseAccount;
using Domain.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using WebApi.Modules.Common;
using WebApi.Modules.Common.FeatureFlags;

[ApiVersion("1.0")]
[FeatureGate(CustomFeature.CloseAccount)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public sealed class AccountsController : ControllerBase, IOutputPort
{
    private readonly Notification _notification;
    private readonly ICloseAccountUseCase _useCase;

    private IActionResult? _viewModel;

    public AccountsController(Notification notification, ICloseAccountUseCase useCase)
    {
        _notification = notification;
        _useCase = useCase;
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

    void IOutputPort.HasFunds()
    {
        _viewModel = BadRequest("Account has funds.");
    }

    void IOutputPort.Ok(Account account)
    {
        _viewModel = Ok(new CloseAccountResponse(account));
    }

    [HttpDelete("{accountId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CloseAccountResponse))]
    [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Delete))]
    public async Task<IActionResult> Close([FromRoute][Required] Guid accountId)
    {
        _useCase.SetOutputPort(this);

        await _useCase
            .Execute(accountId)
            .ConfigureAwait(false);

        return _viewModel!;
    }
}