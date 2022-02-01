namespace Domain.Accounts;

using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;

public sealed class AccountNull : IAccount
{
    public AccountId AccountId => new(Guid.Empty);

    public void Deposit(Credit credit)
    {
        // Null Pattern
    }

    public void Withdraw(Debit debit)
    {
        // Null Pattern
    }

    public Money GetCurrentBalance() => new(0, new Currency());

    public bool IsClosingAllowed() => false;

    public static AccountNull Instance { get; } = new();
}
