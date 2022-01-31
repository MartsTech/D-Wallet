namespace WebApi.UseCases.Accounts.GetAccount;

using System.ComponentModel.DataAnnotations;
using System.Data;
using Application.Services;
using Application.UseCases.GetAccount;
using Domain.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using OfficeOpenXml;
using WebApi.Modules.Common.FeatureFlags;

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
        using DataTable dataTable = new();

        dataTable.Columns.Add("AccountId", typeof(Guid));
        dataTable.Columns.Add("Amount", typeof(decimal));

        dataTable.Rows.Add(account.AccountId.Id, account.GetCurrentBalance().Amount);

        byte[] fileContents;

        using (ExcelPackage pck = new())
        {
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(account.AccountId.ToString());
            ws.Cells["A1"].LoadFromDataTable(dataTable, true);
            ws.Row(1).Style.Font.Bold = true;
            ws.Column(3).Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
            fileContents = pck.GetAsByteArray();
        }

        _viewModel = new FileContentResult(fileContents,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

    }

    [Authorize]
    [HttpGet("{AccountId:guid}", Name = "GetAccount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(
    [FromServices] IGetAccountUseCase useCase,
    [FromRoute][Required] GetAccountDetailsRequest request)
    {
        useCase.SetOutputPort(this);

        await useCase
            .Execute(request.AccountId)
            .ConfigureAwait(false);

        return _viewModel!;
    }
}
