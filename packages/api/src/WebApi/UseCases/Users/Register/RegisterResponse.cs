namespace WebApi.UseCases.Users.Register;

using WebApi.UseCases.Users;

public sealed class RegisterResponse
{
    public RegisterResponse(UserDto user)
    {
        User = user;
    }

    public UserDto User { get; }
}