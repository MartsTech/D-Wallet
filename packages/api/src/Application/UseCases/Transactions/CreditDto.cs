namespace Application.UseCases.Transactions;

using Domain.Credits;

public sealed class CreditDto
{
    public CreditDto(Credit credit)
    {
        Id = credit.AccountId.Id;
        Amount = credit.Amount.Amount;
        Currency = credit.Amount.Currency.Code;
        Description = "Credit";
        TransactionDate = credit.TransactionDate;
    }

    public Guid Id { get; }

    public decimal Amount { get; }

    public string Currency { get; }

    public string Description { get; }

    public DateTime TransactionDate { get; }
}