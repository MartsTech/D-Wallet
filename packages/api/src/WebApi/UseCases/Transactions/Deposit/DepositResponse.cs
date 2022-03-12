namespace WebApi.UseCases.Transactions.Deposit;

using Application.UseCases.Transactions;

public sealed class DepositResponse
{
    public DepositResponse(CreditDto credit)
    {
        Credit = credit;
    }

    public CreditDto Credit { get; }
}