namespace Application.UseCases.Transactions;

using Domain.Debits;

public sealed class DebitDto
{
    public DebitDto(Debit debit)
    {
        Id = debit.DebitId.Id;
        Amount = debit.Amount.Amount;
        Currency = debit.Amount.Currency.Code;
        Description = "Debit";
        TransactionDate = debit.TransactionDate;
    }

    public Guid Id { get; }

    public decimal Amount { get; }

    public string Currency { get; }

    public string Description { get; }

    public DateTime TransactionDate { get; }
}