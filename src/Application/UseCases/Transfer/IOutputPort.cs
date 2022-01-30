using Domain.Accounts;
using Domain.Credits;
using Domain.Debits;

namespace Application.UseCases.Transfer;

public interface IOutputPort
{
    void Invalid();

    void NotFound();

    void Ok(Account originAccount, Debit debit, Account destinationAccount, Credit credit);

    void OutOfFunds();
}
