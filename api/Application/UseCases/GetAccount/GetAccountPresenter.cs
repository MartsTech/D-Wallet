using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.GetAccount;

using Domain.Accounts;

public sealed class GetAccountPresenter : IOutputPort
{
    public Account? Account { get; private set; }
    public bool? IsNotFound { get; private set; }
    public bool? InvalidOutput { get; private set; }

    public void Invalid()
    {
        InvalidOutput = true;
    }

    public void NotFound()
    {
        IsNotFound = true;
    }

    public void Ok(Account account)
    {
        Account = account;
    }
}
