namespace WebApi.UseCases.Users.Login;

using System.ComponentModel.DataAnnotations;

public sealed class LoginInput
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex")]
    public string Password { get; set; } = string.Empty;
}