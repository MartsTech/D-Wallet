namespace Application.UseCases.Transactions.Transfer;

using System.ComponentModel.DataAnnotations;

public sealed class TransferInput
{
    [Required]
    public Guid OriginAccountId { get; set; }

    [Required]
    public Guid DestinationAccountId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public string Currency { get; set; } = string.Empty;
}