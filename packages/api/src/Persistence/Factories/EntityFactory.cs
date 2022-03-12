namespace Persistence.Factories;

using Domain;
using Domain.Accounts;
using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;

public sealed class EntityFactory : IEntityFactory
{
    public Account NewAccount(string userId, Currency currency)
    {
        return new Account(new AccountId(Guid.NewGuid()), userId, currency);
    }

    public Credit NewCredit(Account account, Money deposit, DateTime transactionDate)
    {
        return new Credit(new CreditId(Guid.NewGuid()), account.AccountId, transactionDate,
            deposit.Amount, deposit.Currency.Code);
    }

    public Debit NewDebit(Account account, Money withdraw, DateTime transactionDate)
    {
        return new Debit(new DebitId(Guid.NewGuid()), account.AccountId, transactionDate,
            withdraw.Amount, withdraw.Currency.Code);
    }
}