namespace Domain.Debits;

using Domain.ValueObjects;

public interface IDebit
{
    DebitId DebitId { get; }

    Money Amount { get; }
}
