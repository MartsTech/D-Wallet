namespace Application.UseCases.CloseAccount;

using Domain.Accounts;

public interface IOutputPort
{
    void Invalid();

    void NotFound();

    void HasFunds();

    void Ok(Account account);
}
