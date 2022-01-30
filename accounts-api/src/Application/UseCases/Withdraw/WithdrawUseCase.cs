using Application.Services;
using Domain.Accounts;
using Domain.Debits;
using Domain.ValueObjects;

namespace Application.UseCases.Withdraw;

public sealed class WithdrawUseCase : IWithdrawUseCase
{
    private readonly IAccountFactory _accountFactory;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyExchange _currencyExchange;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private IOutputPort _outputPort;

    public WithdrawUseCase(IAccountFactory accountFactory, IAccountRepository accountRepository, ICurrencyExchange currencyExchange, IUnitOfWork unitOfWork, IUserService userService)
    {
        _accountFactory = accountFactory;
        _accountRepository = accountRepository;
        _currencyExchange = currencyExchange;
        _unitOfWork = unitOfWork;
        _userService = userService;
        _outputPort = new WithdrawPresenter();
    }

    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
    }

    public Task Execute(Guid accountId, decimal amount, string currency)
    {
        return Withdraw(
            new AccountId(accountId),
            new Money(amount, new Currency(currency)));
    }

    private async Task Withdraw(AccountId accountId, Money withdrawAmount)
    {
        string externalUserId = _userService
          .GetCurrentUserId();

        IAccount account = await _accountRepository
            .Find(accountId, externalUserId)
            .ConfigureAwait(false);

        if (account is Account withdrawAccount)
        {
            Money localCurrencyAmount =
                await _currencyExchange
                    .Convert(withdrawAmount, withdrawAccount.Currency)
                    .ConfigureAwait(false);

            Debit debit = _accountFactory
                .NewDebit(withdrawAccount, localCurrencyAmount, DateTime.Now);

            if (withdrawAccount.GetCurrentBalance().Subtract(debit.Amount).Amount < 0)
            {
                _outputPort?.OutOfFunds();
                return;
            }

            await Withdraw(withdrawAccount, debit)
                .ConfigureAwait(false);

            _outputPort.Ok(debit, withdrawAccount);
            return;
        }

        _outputPort.NotFound();
    }

    private async Task Withdraw(Account account, Debit debit)
    {
        account.Withdraw(debit);

        await _accountRepository
            .Update(account, debit)
            .ConfigureAwait(false);

        await _unitOfWork
           .Save()
           .ConfigureAwait(false);
    }
}
