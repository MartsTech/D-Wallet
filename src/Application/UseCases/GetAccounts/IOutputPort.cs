using Domain.Accounts;

namespace Application.UseCases.GetAccounts;

public interface IOutputPort
{
    void Ok(IList<Account> accounts);
}
