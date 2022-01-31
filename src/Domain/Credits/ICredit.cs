namespace Domain.Credits;

using Domain.ValueObjects;

public interface ICredit
{
    CreditId CreditId { get; }

    Money Amount { get; }
}
