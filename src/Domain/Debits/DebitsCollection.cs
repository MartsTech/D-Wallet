using Domain.ValueObjects;

namespace Domain.Debits;

public sealed class DebitsCollection : List<Debit>
{
    public Money GetTotal()
    {
        if (Count == 0)
        {
            return new Money(0, new Currency(string.Empty));
        }

        Money total = new(0, this.First().Amount.Currency);

        return this.Aggregate(total, (current, credit) =>
            new Money(current.Amount + credit.Amount.Amount, current.Currency));
    }
}
