namespace Application.UseCases.Deposit;

using Application.Services;
using Domain.Accounts;
using Domain.Credits;
using Domain.ValueObjects;

public sealed class DepositUseCase : IDepositUseCase
{
    private readonly IAccountFactory _accountFactory;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyExchange _currencyExchange;
    private readonly IUnitOfWork _unitOfWork;
    private IOutputPort _outputPort;

    public DepositUseCase(IAccountFactory accountFactory, IAccountRepository accountRepository, ICurrencyExchange currencyExchange, IUnitOfWork unitOfWork)
    {
        _accountFactory = accountFactory;
        _accountRepository = accountRepository;
        _currencyExchange = currencyExchange;
        _unitOfWork = unitOfWork;
        _outputPort = new DepositPresenter();
    }

    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
    }

    public Task Execute(Guid accountId, decimal amount, string currency)
    {
        return Deposit(
            new AccountId(accountId),
            new Money(amount, new Currency(currency)));
    }

    private async Task Deposit(AccountId accountId, Money amount)
    {
        IAccount account = await _accountRepository
            .GetAccount(accountId)
            .ConfigureAwait(false);

        if (account is Account depositAccount)
        {
            Money convertedAmount =
                await _currencyExchange
                    .Convert(amount, depositAccount.Currency)
                    .ConfigureAwait(false);

            Credit credit = _accountFactory
                .NewCredit(depositAccount, convertedAmount, DateTime.Now);

            await Deposit(depositAccount, credit)
                .ConfigureAwait(false);

            _outputPort.Ok(credit, depositAccount);
            return;
        }
    }

    private async Task Deposit(Account account, Credit credit)
    {
        account.Deposit(credit);

        await _accountRepository
            .Update(account, credit)
            .ConfigureAwait(false);

        await _unitOfWork
          .Save()
          .ConfigureAwait(false);
    }
}
