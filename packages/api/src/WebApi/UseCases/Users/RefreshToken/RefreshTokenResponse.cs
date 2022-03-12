namespace WebApi.UseCases.Users.RefreshToken;

public sealed class RefreshTokenResponse
{
    public RefreshTokenResponse(UserDto user)
    {
        User = user;
    }

    public UserDto User { get; }
}