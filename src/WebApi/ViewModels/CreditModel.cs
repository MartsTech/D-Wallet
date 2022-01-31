namespace WebApi.ViewModels;

using Domain.Credits;
using System.ComponentModel.DataAnnotations;

public sealed class CreditModel
{
    public CreditModel(Credit credit)
    {
        TransactionId = credit.CreditId.Id;
        Amount = credit.Amount.Amount;
        Currency = credit.Amount.Currency.Code;
        TransactionDate = credit.TransactionDate;
        Description = "Credit";
    }

    [Required]
    public Guid TransactionId { get; }

    [Required]
    public decimal Amount { get; }

    [Required]
    public string Currency { get; }

    [Required]
    public string Description { get; }

    [Required]
    public DateTime TransactionDate { get; }
}
