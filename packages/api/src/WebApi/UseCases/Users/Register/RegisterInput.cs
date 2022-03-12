namespace WebApi.UseCases.Users.Register;

using System.ComponentModel.DataAnnotations;

public class RegisterInput
{

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be 8 characters long, contain at least one number and have a mixture of uppercase and lowercase letters.")]
    public string Password { get; set; }

    [Required]
    public string Username { get; set; }
}