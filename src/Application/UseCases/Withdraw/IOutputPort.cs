using Domain.Accounts;
using Domain.Debits;

namespace Application.UseCases.Withdraw;

public interface IOutputPort
{
    void OutOfFunds();

    void Invalid();

    void NotFound();

    void Ok(Debit debit, Account account);
}
