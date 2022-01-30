namespace Domain.ValueObjects;

public readonly struct Currency : IEquatable<Currency>
{
    public string Code { get; }

    public Currency(string code)
    {
        Code = code;
    }

    public bool Equals(Currency other)
    {
        return Code == other.Code;
    }

    public override bool Equals(object? obj)
    {
        return obj is Currency o && Equals(o);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Code);
    }

    public override string ToString()
    {
        return Code;
    }

    public static bool operator ==(Currency left, Currency right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Currency left, Currency right)
    {
        return !left.Equals(right);
    }

    public static readonly Currency Dollar = new ("USD");

    public static readonly Currency Euro = new ("EUR");

    public static readonly Currency BritishPound = new ("GBP");

    public static readonly Currency Canadian = new ("CAD");

    public static readonly Currency Real = new ("BRL");

    public static readonly Currency Krona = new ("SEK");
}

