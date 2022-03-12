namespace Domain.Credits;

using Domain.ValueObjects;

public sealed class CreditNull : ICredit
{
    public CreditId CreditId { get; } = new(Guid.Empty);

    public Money Amount { get; } = new(0, new Currency(string.Empty));

    public static CreditNull Instance { get; } = new();
}