using Domain.Accounts;

namespace Application.UseCases.GetAccounts;

public sealed class GetAccountPresenter : IOutputPort
{
    public IList<Account>? Accounts { get; private set; }

    public void Ok(IList<Account> accounts)
    {
        Accounts = accounts;
    }
}
