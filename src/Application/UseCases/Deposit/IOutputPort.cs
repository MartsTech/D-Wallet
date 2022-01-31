namespace Application.UseCases.Deposit;

using Domain.Accounts;
using Domain.Credits;

public interface IOutputPort
{
    void Invalid();

    void Ok(Credit credit, Account account);

    void NotFound();
}
