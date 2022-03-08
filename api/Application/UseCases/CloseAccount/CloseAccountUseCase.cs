using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Domain.Accounts;
using Domain.ValueObjects;

namespace Application.UseCases.CloseAccount;

public sealed class CloseAccountUseCase : ICloseAccountUseCase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private IOutputPort _outputPort;

    public CloseAccountUseCase(IAccountRepository accountRepository, IUnitOfWork unitOfWork, IUserService userService)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _userService = userService;
        _outputPort = new CloseAccountPresenter();
    }

    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
    }

    public Task Execute(Guid accountId)
    {
        string externalUserId = _userService.GetCurrentUserId();

        return CloseAccountInternal(new AccountId(accountId), externalUserId);

    }

    private async Task CloseAccountInternal(AccountId accountId, string externalUserId)
    {
        IAccount account = await _accountRepository
            .Find(accountId, externalUserId)
            .ConfigureAwait(false);

        if (account is Account closingAccount)
        {
            if (!closingAccount.IsClosingAllowed())
            {
                _outputPort.HasFunds();
                return;
            }

            await Close(closingAccount).ConfigureAwait(false);

            _outputPort.Ok(closingAccount);
            return;
        }
    }

    private async Task Close(Account closeAccount)
    {
        await _accountRepository
            .Delete(closeAccount.AccountId)
            .ConfigureAwait(false);

        await _unitOfWork
            .Save()
            .ConfigureAwait(false);
    }
}
