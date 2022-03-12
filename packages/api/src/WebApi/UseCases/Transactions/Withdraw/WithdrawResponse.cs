namespace WebApi.UseCases.Transactions.Withdraw;

using Application.UseCases.Transactions;

public sealed class WithdrawResponse
{
    public WithdrawResponse(DebitDto debit)
    {
        Debit = debit;
    }

    public DebitDto Debit { get; }
}