namespace Application.UseCases.Accounts.OpenAccount;

using System.ComponentModel.DataAnnotations;

public sealed class OpenAccountInput
{
    [Required]
    public decimal Amount { get; set; }

    [Required]
    public string Currency { get; set; } = string.Empty;
}