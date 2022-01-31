namespace WebApi.UseCases.Transactions.Withdraw;

using System.ComponentModel.DataAnnotations;
using WebApi.ViewModels;

public sealed class WithdrawResponse
{
    public WithdrawResponse(DebitModel transaction)
    {
        Transaction = transaction;
    }

    [Required]
    public DebitModel Transaction { get; }
}
