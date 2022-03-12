namespace Domain.ValueObjects;

public readonly struct Money : IEquatable<Money>
{
    public decimal Amount { get; }

    public Currency Currency { get; }

    public Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public bool IsZero()
    {
        return Amount == 0;
    }

    public Money Add(Money amount)
    {
        return new Money(Math.Round(Amount + amount.Amount, 2), Currency);
    }

    public Money Subtract(Money debit)
    {
        return new Money(Math.Round(Amount - debit.Amount, 2), Currency);
    }

    public bool Equals(Money other)
    {
        return Amount == other.Amount && Currency == other.Currency;
    }

    public override bool Equals(object? obj)
    {
        return obj is Money o && Equals(o);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }

    public override string ToString()
    {
        return string.Format($"{Amount} {Currency}");
    }

    public static bool operator ==(Money left, Money right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Money left, Money right)
    {
        return !left.Equals(right);
    }
}