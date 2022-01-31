namespace Application.UseCases.GetAccounts;

using Domain.Accounts;

public interface IOutputPort
{
    void Ok(IList<Account> accounts);
}
