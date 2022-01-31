namespace WebApi.UseCases.Transactions.Deposit;

using System.ComponentModel.DataAnnotations;
using WebApi.ViewModels;

public sealed class DepositResponse
{
    public DepositResponse(CreditModel transaction)
    {
        Transaction = transaction;
    }

    [Required]
    public CreditModel Transaction { get; }
}
