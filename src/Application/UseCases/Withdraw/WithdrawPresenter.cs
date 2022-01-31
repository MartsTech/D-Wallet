using Domain.Accounts;
using Domain.Debits;

namespace Application.UseCases.Withdraw;

public sealed class WithdrawPresenter : IOutputPort
{
    public Account? Account { get; private set; }

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

    public void Ok(Debit debit, Account account)
    {
        Account = account;
        Debit = debit;
    }

    public void OutOfFunds()
    {
        OutOfFundsOutput = true;
    }
}
