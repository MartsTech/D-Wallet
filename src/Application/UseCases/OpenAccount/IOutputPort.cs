using Domain.Accounts;

namespace Application.UseCases.OpenAccount;

public interface IOutputPort
{
    void Ok(Account account);

    void NotFound();

    void Invalid();
}
