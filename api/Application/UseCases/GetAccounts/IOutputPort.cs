using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.GetAccounts;

using Domain.Accounts;

public interface IOutputPort
{
    void Ok(IList<Account> accounts);
}
