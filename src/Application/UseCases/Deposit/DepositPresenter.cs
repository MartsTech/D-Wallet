using Domain.Accounts;
using Domain.Credits;

namespace Application.UseCases.Deposit;

public sealed class DepositPresenter : IOutputPort
{
    public Account? Account { get; private set; }

    public Credit? Credit { get; private set; }

    public bool? IsNotFound { get; private set; }

    public bool? InvalidOutput { get; private set; }

    public void Invalid()
    {
        InvalidOutput = true;
    }

    public void NotFound()
    {
        IsNotFound = true;
    }

    public void Ok(Credit credit, Account account)
    {
        Credit = credit;
        Account = account;
    }
}
