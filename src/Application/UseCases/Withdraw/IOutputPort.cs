namespace Application.UseCases.Withdraw;

using Domain.Accounts;
using Domain.Debits;

public interface IOutputPort
{
    void OutOfFunds();

    void Invalid();

    void NotFound();

    void Ok(Debit debit, Account account);
}
