namespace WebApi.UseCases.Transactions.Transfer;

using System.ComponentModel.DataAnnotations;
using WebApi.ViewModels;

public sealed class TransferResponse
{
    public TransferResponse(DebitModel transaction)
    {
        Transaction = transaction;
    }

    [Required]
    public DebitModel Transaction { get; }
}
