using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.ViewModels;

using Domain.Debits;
using System.ComponentModel.DataAnnotations;

public sealed class DebitModel
{
    public DebitModel(Debit credit)
    {
        TransactionId = credit.DebitId.Id;
        Amount = credit.Amount.Amount;
        Currency = credit.Amount.Currency.Code;
        TransactionDate = credit.TransactionDate;
        Description = "Debit";
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
