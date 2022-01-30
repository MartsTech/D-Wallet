using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;

namespace Domain.Accounts;

public interface IAccount
{
    AccountId AccountId { get; }

    void Deposit(Credit credit);

    void Withdraw(Debit debit);

    bool IsClosingAllowed();

    Money GetCurrentBalance();
}

