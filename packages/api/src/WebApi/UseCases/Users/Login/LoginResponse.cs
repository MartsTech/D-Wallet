namespace WebApi.UseCases.Users.Login;

using WebApi.UseCases.Users;

public sealed class LoginResponse
{
    public LoginResponse(UserDto user)
    {
        User = user;
    }

    public UserDto User { get; }
}