using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.OpenAccount;

using Domain.Accounts;

public sealed class OpenAccountPresenter : IOutputPort
{
    public Account? Account { get; private set; }

    public bool InvalidOutput { get; private set; }

    public bool NotFoundOutput { get; private set; }

    public void Invalid()
    {
        InvalidOutput = true;
    }

    public void NotFound()
    {
        NotFoundOutput = true;
    }

    public void Ok(Account account)
    {
        Account = account;
    }
}