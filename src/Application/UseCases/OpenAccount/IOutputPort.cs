namespace Application.UseCases.OpenAccount;

using Domain.Accounts;

public interface IOutputPort
{
    void Ok(Account account);

    void NotFound();

    void Invalid();
}
