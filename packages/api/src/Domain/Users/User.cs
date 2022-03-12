namespace Domain.Users;

using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();
}