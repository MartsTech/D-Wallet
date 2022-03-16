namespace Domain.Credits;

using Domain.Accounts;
using Domain.ValueObjects;

public class Credit : ICredit
{
    public Credit(
        CreditId creditId, AccountId accountId, DateTime transactionDate,
        decimal value, string currency)
    {
        CreditId = creditId;
        AccountId = accountId;
        TransactionDate = transactionDate;
        Amount = new Money(value, new Currency(currency));
    }

    public CreditId CreditId { get; }

    public AccountId AccountId { get; }

    public Account? Account { get; }

    public DateTime TransactionDate { get; }

    public Money Amount { get; }

    public decimal Value => Amount.Amount;

    public string Currency => Amount.Currency.Code;

    public static string Description => "Credit";

    public Money Sum(Money amount)
    {
        return Amount.Add(amount);
    }
}