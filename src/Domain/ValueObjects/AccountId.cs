namespace Domain.ValueObjects;

public readonly struct AccountId : IEquatable<AccountId>
{
    public Guid Id { get; }

    public AccountId(Guid id)
    {
        Id = id;
    }

    public bool Equals(AccountId other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is AccountId o && Equals(o);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    public override string ToString()
    {
        return Id.ToString();
    }

    public static bool operator ==(AccountId left, AccountId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(AccountId left, AccountId right)
    {
        return !left.Equals(right);
    }
}
