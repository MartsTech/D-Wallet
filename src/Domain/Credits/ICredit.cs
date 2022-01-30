using Domain.ValueObjects;

namespace Domain.Credits;

public interface ICredit
{
    CreditId CreditId { get; }

    Money Amount { get; }
}
