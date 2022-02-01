﻿namespace Domain.Accounts;

using Domain.Credits;
using Domain.Debits;
using Domain.ValueObjects;

public class Account : IAccount
{
    public Account(AccountId accountId, string externalUserId, Currency currency)
    {
        AccountId = accountId;
        ExternalUserId = externalUserId;
        Currency = currency;
    }

    public AccountId AccountId { get; }

    public string ExternalUserId { get; }

    public Currency Currency { get; }

    public CreditsCollection CreditsCollection { get; } = new();

    public DebitsCollection DebitsCollection { get; } = new();

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
