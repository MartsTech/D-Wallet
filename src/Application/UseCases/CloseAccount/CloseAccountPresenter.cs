using Domain.Accounts;

namespace Application.UseCases.CloseAccount;

public sealed class CloseAccountPresenter : IOutputPort
{
    public Account? Account { get; private set; }

    public bool NotFoundOutput { get; private set; }

    public bool HasFundsOutput { get; private set; }

    public bool InvalidOutput { get; private set; }

    public void HasFunds()
    {
        HasFundsOutput = true;
    }

    public void Invalid()
    {
        InvalidOutput = true;
    }

    public void NotFound()
    {
        NotFoundOutput = true;
    }

    public void Ok(Account account)
    {
        Account = account;
    }
}
