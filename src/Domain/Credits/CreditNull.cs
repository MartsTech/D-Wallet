using Domain.ValueObjects;

namespace Domain.Credits;

public sealed class CreditNull : ICredit
{
    public CreditId CreditId { get; } = new CreditId(Guid.Empty);

    public Money Amount { get; } = new Money(0, new Currency(string.Empty));

    public static CreditNull Instance { get; } = new CreditNull();
}
