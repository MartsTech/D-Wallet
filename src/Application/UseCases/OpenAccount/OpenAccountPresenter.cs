using Domain.Accounts;

namespace Application.UseCases.OpenAccount;

public sealed class OpenAccountPresenter : IOutputPort
{
    public Account? Account { get; private set; }

    public bool InvalidOutput { get; private set; }

    public bool NotFoundOutput { get; private set; }

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
