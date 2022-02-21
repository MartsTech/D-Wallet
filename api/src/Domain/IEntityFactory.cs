namespace Domain;

using Domain.Accounts;
using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;

public interface IEntityFactory
{
    Account NewAccount(string externalUserId, Currency currency);

    Credit NewCredit(Account account, Money amountToDeposit, DateTime transactionDate);

    Debit NewDebit(Account account, Money amountToWithdraw, DateTime transactionDate);
}