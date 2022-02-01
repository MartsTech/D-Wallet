namespace Domain.Accounts;

using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;

public interface IAccount
{
    AccountId AccountId { get; }

    void Deposit(Credit credit);

    void Withdraw(Debit debit);

    Money GetCurrentBalance();

    bool IsClosingAllowed();
}