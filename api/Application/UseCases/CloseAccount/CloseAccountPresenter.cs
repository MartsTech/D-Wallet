using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.UseCases.CloseAccount;

using Domain.Accounts;

public sealed class CloseAccountPresenter : IOutputPort
{
    public Account? Account { get; private set; }

    public bool NotFoundOutput { get; private set; }

    public bool HasFundsOutput { get; private set; }

    public bool InvalidOutput { get; private set; }

    public void Invalid()
    {
        InvalidOutput = true;
    }

    public void NotFound()
    {
        NotFoundOutput = true;
    }

    public void HasFunds()
    {
        HasFundsOutput = true;
    }

    public void Ok(Account account)
    {
        Account = account;
    }
}
