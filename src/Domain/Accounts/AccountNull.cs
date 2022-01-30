using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;

namespace Domain.Accounts;

public sealed class AccountNull : IAccount
{
    public AccountId AccountId => new AccountId(Guid.Empty);

    public void Deposit(Credit credit)
    {
        // Null Pattern
    }

    public void Withdraw(Debit debit)
    {
        // Null Pattern
    }

    public Money GetCurrentBalance() => new Money(0, new Currency());

    public bool IsClosingAllowed() => false;
}
