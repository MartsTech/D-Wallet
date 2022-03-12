namespace WebApi.UseCases.Users;

using Domain.Users;

public sealed class UserDto
{
    public UserDto(User user, string token)
    {
        Id = user.Id;
        Token = token;
    }

    public string Id { get; }

    public string Token { get; }
}