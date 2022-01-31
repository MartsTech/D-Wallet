namespace Application.UseCases.GetAccounts;

using Domain.Accounts;

public sealed class GetAccountPresenter : IOutputPort
{
    public IList<Account>? Accounts { get; private set; }

    public void Ok(IList<Account> accounts)
    {
        Accounts = accounts;
    }
}
