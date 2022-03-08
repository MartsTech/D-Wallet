using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.OpenAccount;

using Application.Services;
using Domain.Accounts;
using Domain.Credits;
using Domain.ValueObjects;

public sealed class OpenAccountUseCase : IOpenAccountUseCase
{
    private readonly IAccountFactory _accountFactory;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private IOutputPort _outputPort;

    public OpenAccountUseCase(IAccountFactory accountFactory, IAccountRepository accountRepository, IUnitOfWork unitOfWork, IUserService userService)
    {
        _accountFactory = accountFactory;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _userService = userService;
        _outputPort = new OpenAccountPresenter();
    }

    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
    }

    public Task Execute(decimal amount, string currency)
    {
        return OpenAccount(new Money(amount, new Currency(currency)));
    }

    private async Task OpenAccount(Money amountToDeposit)
    {
        string externalUserId = _userService.GetCurrentUserId();

        Account account = _accountFactory
           .NewAccount(externalUserId, amountToDeposit.Currency);

        Credit credit = _accountFactory
            .NewCredit(account, amountToDeposit, DateTime.Now);

        await Deposit(account, credit)
            .ConfigureAwait(false);

        _outputPort.Ok(account);
    }

    private async Task Deposit(Account account, Credit credit)
    {
        account.Deposit(credit);

        await _accountRepository
            .Add(account, credit)
            .ConfigureAwait(false);

        await _unitOfWork
            .Save()
            .ConfigureAwait(false);
    }
}