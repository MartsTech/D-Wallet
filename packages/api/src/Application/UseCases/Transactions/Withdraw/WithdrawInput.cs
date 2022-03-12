namespace Application.UseCases.Transactions.Withdraw;

using System.ComponentModel.DataAnnotations;

public sealed class WithdrawInput
{
    [Required]
    public Guid AccountId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public string Currency { get; set; } = string.Empty;
}