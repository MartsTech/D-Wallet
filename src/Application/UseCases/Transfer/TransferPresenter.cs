namespace Application.UseCases.Transfer;

using Domain.Accounts;
using Domain.Credits;
using Domain.Debits;

public sealed class TransferPresenter : IOutputPort
{
    public Account? OriginAccount { get; private set; }

    public Account? DestinationAccount { get; private set; }

    public Credit? Credit { get; private set; }

    public Debit? Debit { get; private set; }

    public bool InvalidOutput { get; private set; }

    public bool NotFoundOutput { get; private set; }

    public bool OutOfFundsOutput { get; private set; }

    public void Invalid()
    {
        InvalidOutput = true;
    }

    public void NotFound()
    {
        NotFoundOutput = true;
    }

    public void Ok(Account originAccount, Debit debit, Account destinationAccount, Credit credit)
    {
        OriginAccount = originAccount;
        Debit = debit;
        DestinationAccount = destinationAccount;
        Credit = credit;
    }

    public void OutOfFunds()
    {
        OutOfFundsOutput = true;
    }
}
