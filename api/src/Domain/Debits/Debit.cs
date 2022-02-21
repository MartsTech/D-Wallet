namespace Domain.Debits;

using Domain.Accounts;
using Domain.ValueObjects;

public class Debit : IDebit
{
    public Debit(DebitId debitId, AccountId accountId, DateTime transactionDate, decimal value, string currency)
    {
        DebitId = debitId;
        AccountId = accountId;
        TransactionDate = transactionDate;
        Amount = new Money(value, new Currency(currency));
    }

    public DebitId DebitId { get; }

    public AccountId AccountId { get; }

    public Account? Account { get; set; }

    public DateTime TransactionDate { get; }

    public Money Amount { get; }

    public decimal Value => Amount.Amount;

    public string Currency => Amount.Currency.Code;

    public static string Description => "Debit";

    public Money Sum(Money amount)
    {
        return Amount.Add(amount);
    }
}