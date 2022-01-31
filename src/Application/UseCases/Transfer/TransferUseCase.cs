using Application.Services;
using Domain.Accounts;
using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;

namespace Application.UseCases.Transfer;

public sealed class TransferUseCase : ITransferUseCase
{
    private readonly IAccountFactory _accountFactory;
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrencyExchange _currencyExchange;
    private readonly IUnitOfWork _unitOfWork;
    private IOutputPort? _outputPort;

    public TransferUseCase(IAccountFactory accountFactory, IAccountRepository accountRepository, ICurrencyExchange currencyExchange, IUnitOfWork unitOfWork)
    {
        _accountFactory = accountFactory;
        _accountRepository = accountRepository;
        _currencyExchange = currencyExchange;
        _unitOfWork = unitOfWork;
        _outputPort = new TransferPresenter();
    }

    public void SetOutputPort(IOutputPort outputPort)
    {
        _outputPort = outputPort;
    }

    public Task Execute(Guid originAccountId, Guid destinationAccountId, decimal amount, string currency)
    {
       return Transfer(
            new AccountId(originAccountId),
            new AccountId(destinationAccountId),
            new Money(amount, new Currency(currency)));
    }

    private async Task Transfer(AccountId originAccountId, AccountId destinationAccountId, Money transferAmount)
    {
        IAccount originAccount = await _accountRepository
            .GetAccount(originAccountId)
            .ConfigureAwait(false);

        IAccount destinationAccount = await _accountRepository
            .GetAccount(destinationAccountId)
            .ConfigureAwait(false);

        if (originAccount is Account withdrawAccount && destinationAccount is Account depositAccount)
        {
            Money localCurrencyAmount =
                await _currencyExchange
                    .Convert(transferAmount, withdrawAccount.Currency)
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

            Money destinationCurrencyAmount =
                await _currencyExchange
                    .Convert(transferAmount, depositAccount.Currency)
                    .ConfigureAwait(false);

            Credit credit = _accountFactory
                .NewCredit(depositAccount, destinationCurrencyAmount, DateTime.Now);

            await Deposit(depositAccount, credit)
                .ConfigureAwait(false);

            _outputPort?.Ok(withdrawAccount, debit, depositAccount, credit);
            return;
        }

        _outputPort?.NotFound();
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
