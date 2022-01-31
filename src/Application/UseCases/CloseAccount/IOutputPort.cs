namespace Application.UseCases.CloseAccount;

using Domain.Accounts;

public interface IOutputPort
{
    void Invalid();

    void Ok(Account account);

    void NotFound();

    void HasFunds();
}
