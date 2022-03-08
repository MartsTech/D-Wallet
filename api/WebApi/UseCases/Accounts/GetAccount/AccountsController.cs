using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.UseCases.Accounts.GetAccount;

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Application.Services;
using Application.UseCases.GetAccount;
using Domain.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Modules.Common.FeatureFlags;
using WebApi.Modules.Common;

[ApiVersion("1.0")]
[FeatureGate(CustomFeature.GetAccount)]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public sealed class AccountsController : ControllerBase, IOutputPort
{
    private readonly Notification _notification;

    private IActionResult? _viewModel;

    public AccountsController(Notification notification)
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

    void IOutputPort.Ok(Account account)
    {
        _viewModel = Ok(new GetAccountResponse(account));
    }

    [HttpGet("{accountId:guid}", Name = "GetAccount")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAccountResponse))]
    [ApiConventionMethod(typeof(CustomApiConventions), nameof(CustomApiConventions.Find))]
    public async Task<IActionResult> Get(
    [FromServices] IGetAccountUseCase useCase,
    [FromRoute][Required] Guid accountId)
    {
        useCase.SetOutputPort(this);

        await useCase.Execute(accountId)
           .ConfigureAwait(false);

        return _viewModel!;
    }
}
