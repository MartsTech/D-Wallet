using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;

namespace Domain.Accounts;

public class Account : IAccount
{
    public Account(AccountId accountId, Currency currency, string externalUserId)
    {
        AccountId = accountId;
        Currency = currency;
        ExternalUserId = externalUserId;
    }

    public AccountId AccountId { get; }

    public Currency Currency { get; }

    public string ExternalUserId { get; }

    public CreditsCollection CreditsCollection { get; } = new CreditsCollection();

    public DebitsCollection DebitsCollection { get; } = new DebitsCollection();

    public void Deposit(Credit credit)
    {
        CreditsCollection.Add(credit);
    }

    public void Withdraw(Debit debit)
    {
        DebitsCollection.Add(debit);
    }

    public Money GetCurrentBalance()
    {
        Money totalCredits = CreditsCollection.GetTotal();

        Money totalDebits = DebitsCollection.GetTotal();

        Money totalAmount = totalCredits.Subtract(totalDebits);

        return totalAmount;
    }

    public bool IsClosingAllowed()
    {
        return GetCurrentBalance().IsZero();
    }
}
