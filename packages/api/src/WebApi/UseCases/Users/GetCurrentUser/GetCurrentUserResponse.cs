namespace WebApi.UseCases.Users.GetCurrentUser;

using WebApi.UseCases.Users;

public sealed class GetCurrentUserResponse
{
    public GetCurrentUserResponse(UserDto user)
    {
        User = user;
    }

    public UserDto User { get; }
}