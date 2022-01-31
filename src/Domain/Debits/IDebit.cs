using Domain.ValueObjects;

namespace Domain.Debits;

public interface IDebit
{
    DebitId DebitId { get; }

    Money Amount { get; }
}
