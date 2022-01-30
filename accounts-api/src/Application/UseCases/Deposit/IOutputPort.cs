using Domain.Accounts;
using Domain.Credits;

namespace Application.UseCases.Deposit;

public interface IOutputPort
{
    void Invalid();

    void Ok(Credit credit, Account account);

    void NotFound();
}
