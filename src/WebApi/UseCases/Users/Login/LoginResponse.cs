namespace WebApi.UseCases.Users.Login;

using System.ComponentModel.DataAnnotations;

public sealed class LoginResponse
{
    public LoginResponse(string token)
    {
        Token = token;
    }

    [Required]
    public string Token { get; set; }
}
