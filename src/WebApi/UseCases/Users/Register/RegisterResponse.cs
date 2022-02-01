namespace WebApi.UseCases.Users.Register;

using System.ComponentModel.DataAnnotations;

public sealed class RegisterResponse
{
    public RegisterResponse(string token)
    {
        Token = token;
    }

    [Required]
    public string Token { get; set; }
}
