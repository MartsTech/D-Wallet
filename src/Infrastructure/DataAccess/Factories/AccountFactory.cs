using Domain.Accounts;
using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;

namespace Infrastructure.DataAccess.Factories;

public sealed class AccountFactory : IAccountFactory
{
    public Account NewAccount(string externalUserId, Currency currency)
    {
        return new Account(new AccountId(Guid.NewGuid()), currency, externalUserId);
    }

    public Credit NewCredit(Account account, Money amountToDeposit, DateTime transactionDate)
    {
        return new Credit(new CreditId(Guid.NewGuid()), account.AccountId, transactionDate, 
            amountToDeposit.Amount, amountToDeposit.Currency.Code);
    }

    public Debit NewDebit(Account account, Money amountToWithdraw, DateTime transactionDate)
    {
        return new Debit(new DebitId(Guid.NewGuid()), account.AccountId, transactionDate, 
            amountToWithdraw.Amount, amountToWithdraw.Currency.Code);
    }
}
