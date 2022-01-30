using Domain.Accounts;

namespace Application.UseCases.GetAccount;

public interface IOutputPort
{
    void Invalid();

    void NotFound();

    void Ok(Account account);
}
