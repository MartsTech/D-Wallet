namespace Application.UseCases.Transactions.Deposit;

using System.ComponentModel.DataAnnotations;

public sealed class DepositInput
{
    [Required]
    public Guid AccountId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public string Currency { get; set; } = string.Empty;
}