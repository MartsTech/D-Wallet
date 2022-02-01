namespace Domain.Users;

public class RefreshToken
{
    public int Id { get; set; }

    public User? User { get; set; }

    public string? Token { get; set; }

    public DateTime? Revoked { get; set; }

    public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);

    public bool IsExpired => DateTime.UtcNow >= Expires;

    public bool IsActive => Revoked == null && !IsExpired;
}
