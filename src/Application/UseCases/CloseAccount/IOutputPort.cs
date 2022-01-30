using Domain.Accounts;

namespace Application.UseCases.CloseAccount;

public interface IOutputPort
{
    void Invalid();

    void Ok(Account account);

    void NotFound();

    void HasFunds();
}
