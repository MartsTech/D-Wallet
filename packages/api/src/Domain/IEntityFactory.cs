namespace Domain;

using Domain.Accounts;
using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;

public interface IEntityFactory
{
    Account NewAccount(string userId, Currency currency);

    Credit NewCredit(Account account, Money deposit, DateTime transactionDate);

    Debit NewDebit(Account account, Money withdraw, DateTime transactionDate);
}