namespace WebApi.UseCases.Transactions.Transfer;

using Application.UseCases.Transactions;

public sealed class TransferResponse
{
    public TransferResponse(DebitDto debit)
    {
        Debit = debit;
    }

    public DebitDto Debit { get; }
}